namespace RAProductsRegestration.Application.Common.Interfaces
    .Integrations.JDE.Suppliers;

public interface ISupplierDataSource
{
    Task<IReadOnlyCollection<ExternalSupplier>>
        GetSuppliersAsync(
            CancellationToken cancellationToken = default);
}