using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ActiveIngredients.Queries.GetAll;
using RAProductsRegestration.Infrastructure.Data;

namespace RAProductsRegestration.Infrastructure.Repositories
{
    internal sealed class ActiveIngredientQueryRepository(
       ApplicationDbContext context)
       : IActiveIngredientQueryRepository
    {
        public async Task<(
    IReadOnlyList<ActiveIngredientLookupResponse> Items,
    int TotalCount)> GetFilteredAsync(
        string? filter,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
        {
            var query = context.ActiveIngredients
                .AsNoTracking()
                .AsQueryable();


            // =====================================================
            // Filter BEFORE projection
            // =====================================================

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var search =
                    filter.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Code.Contains(search));
            }


            // =====================================================
            // Count filtered entities
            // =====================================================

            var totalCount =
                await query.CountAsync(ct);


            // =====================================================
            // Sort + Pagination + Projection
            // =====================================================

            var items = await query
                .OrderBy(x => x.Name)
                .Skip(
                    (pageNumber - 1) *
                    pageSize
                )
                .Take(pageSize)
                .Select(x =>
                    new ActiveIngredientLookupResponse(
                        x.Id,
                        x.CTDId,
                        x.Code,
                        x.Name))
                .ToListAsync(ct);


            return (
                items,
                totalCount
            );
        }
    }
}
