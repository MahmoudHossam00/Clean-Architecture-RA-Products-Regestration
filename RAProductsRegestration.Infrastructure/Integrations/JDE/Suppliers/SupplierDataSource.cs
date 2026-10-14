using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces
    .Integrations.JDE.Suppliers;

namespace RAProductsRegestration.Infrastructure
    .Integrations.JDE.Suppliers;

internal sealed class SupplierDataSource(
    JdeReadDbContext context)
    : ISupplierDataSource
{
    public async Task<IReadOnlyCollection<ExternalSupplier>>
        GetSuppliersAsync(
            CancellationToken cancellationToken = default)
    {
        var rows = await context.Suppliers
            .FromSqlRaw(
                """
                SELECT
                    ABAN8 AS CODE,
                    ABALPH AS NAME
                FROM PRODDTA.F0101
                WHERE ABAT1 = 'V2'
                """)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows
    .Select(x =>
        new ExternalSupplier(
            x.Code,
            x.Name.Trim()))
    .ToList();
    }
}