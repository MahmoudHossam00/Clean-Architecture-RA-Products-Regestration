using RAProductsRegestration.Application.Common.Interfaces
    .Integrations.JDE.Suppliers;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Suppliers;

namespace RAProductsRegestration.Application.Integrations
    .JDE.Suppliers.Sync;

internal sealed class Handler(
    ISupplierDataSource supplierDataSource,
    IRepository<Supplier> supplierRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<SyncSuppliersCommand, Success>
{
    public async Task<Result<Success>> Handle(
        SyncSuppliersCommand request,
        CancellationToken cancellationToken)
    {
        var externalSuppliers =
            await supplierDataSource.GetSuppliersAsync(
                cancellationToken);

        if (externalSuppliers.Count == 0)
        {
            return Result.Success;
        }

        // Validate external JDE data before syncing.
        foreach (var supplier in externalSuppliers)
        {
            if (supplier.Code <= 0)
            {
                return SyncSuppliersErrors
                    .InvalidJdeCode(
                        supplier.Code);
            }

            if (string.IsNullOrWhiteSpace(
                    supplier.Name))
            {
                return SyncSuppliersErrors
                    .NameRequired(
                        supplier.Code);
            }
        }

        // Protect against duplicate ABAN8 values.
        var externalByCode =
            externalSuppliers
                .GroupBy(x => x.Code)
                .ToDictionary(
                    x => x.Key,
                    x => x.First());

        var jdeCodes =
            externalByCode.Keys.ToArray();

        // One query against our local RA database.
        var existingSuppliers =
            await supplierRepository.WhereAsync(
                x => jdeCodes.Contains(
                    x.JdeCode),
                cancellationToken);

        var existingByCode =
            existingSuppliers.ToDictionary(
                x => x.JdeCode);

        var newSuppliers =
            new List<Supplier>();

        foreach (var externalSupplier
                 in externalByCode.Values)
        {
            if (existingByCode.TryGetValue(
                    externalSupplier.Code,
                    out var supplier))
            {
                supplier.UpdateFromJde(
                    externalSupplier.Name);

                continue;
            }

            var newSupplier =
                new Supplier(
                    externalSupplier.Code,
                    externalSupplier.Name);

            newSuppliers.Add(
                newSupplier);

            // Keeps our in-memory lookup consistent.
            existingByCode.Add(
                externalSupplier.Code,
                newSupplier);
        }

        if (newSuppliers.Count > 0)
        {
            supplierRepository.AddRange(
                newSuppliers);
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success;
    }
}