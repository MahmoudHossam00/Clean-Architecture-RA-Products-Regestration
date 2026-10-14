using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients
    .ApprovedSuppliers;

public sealed class ApprovedSupplier : Entity
{
    private ApprovedSupplier()
    {
    }

    private ApprovedSupplier(
        Guid supplierId)
        : base(Guid.NewGuid())
    {
        SupplierId = supplierId;
        IsActive = true;
    }

    public Guid SupplierId { get; private set; }

    public bool IsActive { get; private set; }

    internal static Result<ApprovedSupplier> Create(
        Guid supplierId)
    {
        if (supplierId == Guid.Empty)
        {
            return ApprovedSupplierErrors
                .SupplierIdRequired;
        }

        return new ApprovedSupplier(
            supplierId);
    }

    internal Result<Updated> Activate()
    {
        IsActive = true;

        return Result.Updated;
    }

    internal Result<Updated> Deactivate()
    {
        IsActive = false;

        return Result.Updated;
    }
}