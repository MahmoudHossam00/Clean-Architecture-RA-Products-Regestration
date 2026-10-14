using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedPacks;
using RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients.ApprovedSuppliers;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients;

public sealed class ProductActiveIngredient : Entity
{
    private readonly List<ApprovedSupplier> _approvedSuppliers = [];


    public Guid? StorageConditionId { get; private set; }
    public string? PhysicalDescription { get; private set; } // string, there is also physicaldescription for activesubstance

    private readonly List<ApprovedPack> _approvedPacks = [];
    public IReadOnlyCollection<ApprovedPack> ApprovedPacks =>
        _approvedPacks.AsReadOnly();

    public ShelfLife? ShelfLife
    {
        get;
        private set;
    }


    private ProductActiveIngredient()
    {
    }


    internal Result<Updated> SetShelfLife(
    int numberOfMonths,
    DateOnly approvalDate,
    IEnumerable<AttachmentReference> attachments)
    {
        var result =
            ShelfLife.Create(
                numberOfMonths,
                approvalDate,
                attachments
            );


        if (result.IsError)
        {
            return result.TopError!;
        }


        ShelfLife =
            result.Value;


        return Result.Updated;
    }

    internal Result<Created> AddShelfLifeAttachment(
    AttachmentReference attachment)
    {
        if (ShelfLife is null)
        {
            return ProductActiveIngredientErrors
                .ShelfLifeNotDefined;
        }


        return ShelfLife
            .AddAttachment(
                attachment
            );
    }

    internal Result<Deleted> RemoveShelfLifeAttachment(
    Guid attachmentId)
    {
        if (ShelfLife is null)
        {
            return ProductActiveIngredientErrors
                .ShelfLifeNotDefined;
        }


        return ShelfLife
            .RemoveAttachment(
                attachmentId
            );
    }

    internal Result<Updated> UpdateStorageCondition(
    Guid? storageConditionId)
    {
        StorageConditionId =
            storageConditionId;

        return Result.Updated;
    }


    internal Result<Updated> UpdatePhysicalDescription(
    string? physicalDescription)
    {
        PhysicalDescription =
            physicalDescription?.Trim();

        return Result.Updated;
    }



    internal Result<Created> AddApprovedPack(
    string description,
    DatedAttachment? approval)
    {
        var result = ApprovedPack.Create(
            description,
            approval);

        if (result.IsError)
        {
            return result.TopError!;
        }

        _approvedPacks.Add(
            result.Value);

        return Result.Created;
    }
    private ProductActiveIngredient(
        Guid activeIngredientId,
        string strength)
        : base(Guid.NewGuid())
    {
        ActiveIngredientId = activeIngredientId;
        Strength = strength;
    }

    public Guid ActiveIngredientId { get; private set; }

    public string Strength { get; private set; } = null!;

    public IReadOnlyCollection<ApprovedSupplier> ApprovedSuppliers =>
        _approvedSuppliers.AsReadOnly();

    internal static Result<ProductActiveIngredient> Create(
        Guid activeIngredientId,
        string strength)
    {
        if (activeIngredientId == Guid.Empty)
        {
            return ProductActiveIngredientErrors
                .ActiveIngredientIdRequired;
        }

        if (string.IsNullOrWhiteSpace(strength))
        {
            return ProductActiveIngredientErrors
                .StrengthRequired;
        }

        return new ProductActiveIngredient(
            activeIngredientId,
            strength.Trim());
    }

    internal Result<Updated> UpdateStrength(
        string strength)
    {
        if (string.IsNullOrWhiteSpace(strength))
        {
            return ProductActiveIngredientErrors
                .StrengthRequired;
        }

        Strength = strength.Trim();

        return Result.Updated;
    }

    internal Result<Created> AddApprovedSupplier(
    Guid supplierId)
    {
        if (_approvedSuppliers.Any(x =>
            x.SupplierId == supplierId))
        {
            return ProductActiveIngredientErrors
                .SupplierAlreadyExists(
                    supplierId);
        }

        var supplierResult =
            ApprovedSupplier.Create(
                supplierId);

        if (supplierResult.IsError)
        {
            return supplierResult.TopError!;
        }

        _approvedSuppliers.Add(
            supplierResult.Value);

        return Result.Created;
    }

    internal Result<Deleted> RemoveApprovedSupplier(
        Guid approvedSupplierId)
    {
        var supplierResult =
            GetApprovedSupplier(approvedSupplierId);

        if (supplierResult.IsError)
        {
            return supplierResult.TopError!;
        }

        _approvedSuppliers.Remove(
            supplierResult.Value);

        return Result.Deleted;
    }

    internal Result<Updated> ActivateApprovedSupplier(
        Guid approvedSupplierId)
    {
        var supplierResult =
            GetApprovedSupplier(approvedSupplierId);

        if (supplierResult.IsError)
        {
            return supplierResult.TopError!;
        }

        return supplierResult.Value.Activate();
    }

    internal Result<Updated> DeactivateApprovedSupplier(
        Guid approvedSupplierId)
    {
        var supplierResult =
            GetApprovedSupplier(approvedSupplierId);

        if (supplierResult.IsError)
        {
            return supplierResult.TopError!;
        }

        return supplierResult.Value.Deactivate();
    }

    private Result<ApprovedSupplier> GetApprovedSupplier(
        Guid approvedSupplierId)
    {
        var approvedSupplier =
            _approvedSuppliers.FirstOrDefault(
                x => x.Id == approvedSupplierId);

        if (approvedSupplier is null)
        {
            return ProductActiveIngredientErrors
                .ApprovedSupplierNotFound(
                    approvedSupplierId);
        }

        return approvedSupplier;
    }
    internal Result<Updated> UpdateApprovedPack(
Guid approvedPackId,
string description,
DatedAttachment? approval)
    {
        var packResult =
            GetApprovedPack(
                approvedPackId);

        if (packResult.IsError)
        {
            return packResult.TopError!;
        }

        return packResult.Value.Update(
            description,
            approval);
    }

    internal Result<Deleted> RemoveApprovedPack(
        Guid approvedPackId)
    {
        var packResult =
            GetApprovedPack(
                approvedPackId);

        if (packResult.IsError)
        {
            return packResult.TopError!;
        }

        _approvedPacks.Remove(
            packResult.Value);

        return Result.Deleted;
    }

    private Result<ApprovedPack> GetApprovedPack(
        Guid approvedPackId)
    {
        var pack =
            _approvedPacks.FirstOrDefault(
                x => x.Id == approvedPackId);

        if (pack is null)
        {
            return ProductActiveIngredientErrors
                .ApprovedPackNotFound(
                    approvedPackId);
        }

        return pack;
    }
}