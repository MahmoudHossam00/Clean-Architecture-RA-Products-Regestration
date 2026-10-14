using Microsoft.EntityFrameworkCore;

using RAProductsRegestration
    .Application.Common.Interfaces.Persistence;

using RAProductsRegestration
    .Application.Features.Suppliers.Queries.GetAll;

using RAProductsRegestration
    .Infrastructure.Data;

namespace RAProductsRegestration
    .Infrastructure.Repositories;

internal sealed class SupplierQueryRepository(
    ApplicationDbContext context)
    : ISupplierQueryRepository
{
    public async Task<(
        IReadOnlyList<SupplierLookupResponse> Items,
        int TotalCount)>
        GetFilteredAsync(
            string? filter,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
    {
        var query =
            context.Suppliers
                .AsNoTracking()
                .AsQueryable();


        // Filter BEFORE DTO projection
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var search =
                filter.Trim();

            if (
                long.TryParse(
                    search,
                    out var jdeCode)
            )
            {
                query =
                    query.Where(x =>
                        x.JdeCode == jdeCode ||
                        x.Name.Contains(search));
            }
            else
            {
                query =
                    query.Where(x =>
                        x.Name.Contains(search));
            }
        }


        var totalCount =
            await query.CountAsync(ct);


        var items =
            await query
                .OrderBy(x => x.Name)
                .Skip(
                    (pageNumber - 1) *
                    pageSize)
                .Take(pageSize)
                .Select(x =>
                    new SupplierLookupResponse(
                        x.Id,
                        x.JdeCode,
                        x.Name))
                .ToListAsync(ct);


        return (
            items,
            totalCount
        );
    }
}