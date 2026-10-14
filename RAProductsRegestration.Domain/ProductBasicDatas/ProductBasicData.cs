using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedManufacturers;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedPacks;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedPrices;
using RAProductsRegestration.Domain.ProductBasicDatas.ProductActiveIngredients;
using RAProductsRegestration.Domain.ProductBasicDatas.RegulatoryDeadlines;
using RAProductsRegestration.Domain.ProductBasicDatas.ShelfLives;

namespace RAProductsRegestration.Domain.ProductBasicDatas;

public sealed class ProductBasicData : AuditableEntity, IAggregateRoot
{
    private readonly List<ProductActiveIngredient> _productActiveIngredients = [];
    private readonly List<ApprovedPack> _approvedPacks = [];

    private readonly List<ApprovedPrice> _approvedPrices = [];
    private readonly List<ApprovedManufacturer> _approvedManufacturers = [];

    private ProductBasicData()
    {
    }

    private ProductBasicData(Guid productId,string? tradeName)
        : base(Guid.NewGuid())
    {
        ProductId = productId;
        TradeName = tradeName;
    }

    private readonly List<PVDeadlineHistoryItem>
    _pvHistory = [];


    private readonly List<Decree600DeadlineHistoryItem>
        _decree600History = [];
    public Guid ProductId { get; private set; }
    public string? TradeName { get; private set; }
    public Guid? DosageFormId { get; private set; }

    public Guid? StorageConditionId { get; private set; }
    public string? PhysicalDescription { get; private set; } // string, there is also physicaldescription for activesubstance



    private readonly List<ShelfLifeHistoryItem>
     _shelfLifeHistory = [];


    public IReadOnlyCollection<ShelfLifeHistoryItem>
    ShelfLifeHistory =>
        _shelfLifeHistory.AsReadOnly();


    // Full current history record.
    // Useful when we need VersionNumber / Id.
    public ShelfLifeHistoryItem?
        CurrentShelfLifeHistory =>
            _shelfLifeHistory
                .OrderByDescending(
                    x => x.VersionNumber)
                .FirstOrDefault();


    // Convenient current ShelfLife value.
    public ShelfLife?
        CurrentShelfLife =>
            CurrentShelfLifeHistory
                ?.ShelfLife;


    // =========================================================
    // PV History
    // =========================================================

    public IReadOnlyCollection<PVDeadlineHistoryItem>
        PVHistory =>
            _pvHistory.AsReadOnly();


    public PVDeadlineHistoryItem?
        CurrentPV =>
            _pvHistory
                .OrderByDescending(
                    x => x.VersionNumber)
                .FirstOrDefault();


    // =========================================================
    // Decree 600 History
    // =========================================================

    public IReadOnlyCollection<Decree600DeadlineHistoryItem>
        Decree600History =>
            _decree600History.AsReadOnly();


    public Decree600DeadlineHistoryItem?
        CurrentDecree600 =>
            _decree600History
                .OrderByDescending(
                    x => x.VersionNumber)
                .FirstOrDefault();

    public IReadOnlyCollection<ProductActiveIngredient> ProductActiveIngredients =>
        _productActiveIngredients.AsReadOnly();

    public IReadOnlyCollection<ApprovedPack> ApprovedPacks =>
        _approvedPacks.AsReadOnly();

    public IReadOnlyCollection<ApprovedPrice> ApprovedPrices =>
        _approvedPrices.AsReadOnly();

    public IReadOnlyCollection<ApprovedManufacturer> ApprovedManufacturers =>
        _approvedManufacturers.AsReadOnly();


    // =========================================================
    // Creation
    // =========================================================

    public static Result<ProductBasicData> Create(Guid productId,string? TradeName)
    {
        if (productId == Guid.Empty)
        {
            return ProductBasicDataErrors.ProductIdRequired;
        }

        return new ProductBasicData(productId, TradeName);
    }


    // =========================================================
    // General Basic Data
    // =========================================================

    public Result<Updated> UpdateDosageForm(
        Guid? dosageFormId)
    {
        DosageFormId = dosageFormId;

        return Result.Updated;
    }

    public Result<Updated> UpdateStorageCondition(
        Guid? storageConditionId)
    {
        StorageConditionId = storageConditionId;

        return Result.Updated;
    }

    public Result<Updated> UpdatePhysicalDescription(
        string? physicalDescription)
    {
        PhysicalDescription = physicalDescription?.Trim();

        return Result.Updated;
    }



    public Result<Created> AddShelfLife(
    int numberOfMonths,
    DateOnly approvalDate,
    IEnumerable<AttachmentReference> attachments)
    {
        var result =
            ShelfLife.Create(
                numberOfMonths,
                approvalDate,
                attachments);


        if (result.IsError)
        {
            return result.TopError!;
        }


        var nextVersion =
            _shelfLifeHistory.Count == 0
                ? 1
                : _shelfLifeHistory.Max(
                    x => x.VersionNumber) + 1;


        var historyItem =
            new ShelfLifeHistoryItem(
                nextVersion,
                result.Value);


        _shelfLifeHistory.Add(
            historyItem);


        return Result.Created;
    }

    public Result<Created> AddShelfLifeAttachment(
    AttachmentReference attachment)
    {
        if (CurrentShelfLife is null)
        {
            return ProductBasicDataErrors
                .ShelfLifeNotDefined;
        }


        return CurrentShelfLife
            .AddAttachment(
                attachment);
    }


    public Result<Deleted> RemoveShelfLifeAttachment(
        Guid attachmentId)
    {
        if (CurrentShelfLife is null)
        {
            return ProductBasicDataErrors
                .ShelfLifeNotDefined;
        }


        return CurrentShelfLife
            .RemoveAttachment(
                attachmentId);
    }


    // =========================================================
    // Active Ingredients
    // =========================================================
    public Result<Created> AddActiveIngredientApprovedPack(
    Guid productActiveIngredientId,
    string description,
    DatedAttachment? approval)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .AddApprovedPack(
                description,
                approval);
    }
    public Result<Created> AddActiveIngredient(
        Guid activeIngredientId,
        string strength)
    {
        if (_productActiveIngredients.Any(x =>
            x.ActiveIngredientId == activeIngredientId))
        {
            return ProductBasicDataErrors
                .ActiveIngredientAlreadyExists(activeIngredientId);
        }

        var ingredientResult =
            ProductActiveIngredient.Create(
                activeIngredientId,
                strength);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        _productActiveIngredients.Add(
            ingredientResult.Value);

        return Result.Created;
    }

    public Result<Updated> UpdateActiveIngredientStrength(
        Guid productActiveIngredientId,
        string strength)
    {
        var ingredientResult =
            GetActiveIngredient(productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .UpdateStrength(strength);
    }

    public Result<Deleted> RemoveActiveIngredient(
        Guid productActiveIngredientId)
    {
        var ingredientResult =
            GetActiveIngredient(productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        _productActiveIngredients.Remove(
            ingredientResult.Value);

        return Result.Deleted;
    }

    private Result<ProductActiveIngredient> GetActiveIngredient(
        Guid productActiveIngredientId)
    {
        var ingredient = _productActiveIngredients
            .FirstOrDefault(x =>
                x.Id == productActiveIngredientId);

        if (ingredient is null)
        {
            return ProductBasicDataErrors
                .ActiveIngredientNotFound(
                    productActiveIngredientId);
        }

        return ingredient;
    }


    // =========================================================
    // Approved Suppliers
    // =========================================================

    public Result<Created> AddApprovedSupplier(
    Guid productActiveIngredientId,
    Guid supplierId)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .AddApprovedSupplier(
                supplierId);
    }

    public Result<Updated> ActivateApprovedSupplier(
        Guid productActiveIngredientId,
        Guid approvedSupplierId)
    {
        var ingredientResult =
            GetActiveIngredient(productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .ActivateApprovedSupplier(
                approvedSupplierId);
    }

    public Result<Updated> DeactivateApprovedSupplier(
        Guid productActiveIngredientId,
        Guid approvedSupplierId)
    {
        var ingredientResult =
            GetActiveIngredient(productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .DeactivateApprovedSupplier(
                approvedSupplierId);
    }

    public Result<Deleted> RemoveApprovedSupplier(
        Guid productActiveIngredientId,
        Guid approvedSupplierId)
    {
        var ingredientResult =
            GetActiveIngredient(productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .RemoveApprovedSupplier(
                approvedSupplierId);
    }


    // =========================================================
    // Approved Manufacturers
    // =========================================================

    public Result<Created>  AddApprovedManufacturer(
        string manufacturerName,DatedAttachment ATT)
    {
        if (!string.IsNullOrWhiteSpace(manufacturerName))
        {
            var normalizedName =
                manufacturerName.Trim();

            if (_approvedManufacturers.Any(x =>
                x.ManufacturerName.Equals(
                    normalizedName,
                    StringComparison.OrdinalIgnoreCase)))
            {
                return ProductBasicDataErrors
                    .ManufacturerAlreadyExists(
                        normalizedName);
            }
        }

        var manufacturerResult =
            ApprovedManufacturer.Create(
                manufacturerName, ATT);

        if (manufacturerResult.IsError)
        {
            return manufacturerResult.TopError!;
        }

        _approvedManufacturers.Add(
            manufacturerResult.Value);

        return Result.Created;
    }

    public Result<Updated> ActivateApprovedManufacturer(
        Guid approvedManufacturerId)
    {
        var manufacturerResult =
            GetApprovedManufacturer(
                approvedManufacturerId);

        if (manufacturerResult.IsError)
        {
            return manufacturerResult.TopError!;
        }

        return manufacturerResult.Value.Activate();
    }

    public Result<Updated> DeactivateApprovedManufacturer(
        Guid approvedManufacturerId)
    {
        var manufacturerResult =
            GetApprovedManufacturer(
                approvedManufacturerId);

        if (manufacturerResult.IsError)
        {
            return manufacturerResult.TopError!;
        }

        return manufacturerResult.Value.Deactivate();
    }

    public Result<Deleted> RemoveApprovedManufacturer(
        Guid approvedManufacturerId)
    {
        var manufacturerResult =
            GetApprovedManufacturer(
                approvedManufacturerId);

        if (manufacturerResult.IsError)
        {
            return manufacturerResult.TopError!;
        }

        _approvedManufacturers.Remove(
            manufacturerResult.Value);

        return Result.Deleted;
    }

    private Result<ApprovedManufacturer> GetApprovedManufacturer(
        Guid approvedManufacturerId)
    {
        var manufacturer = _approvedManufacturers
            .FirstOrDefault(x =>
                x.Id == approvedManufacturerId);

        if (manufacturer is null)
        {
            return ProductBasicDataErrors
                .ApprovedManufacturerNotFound(
                    approvedManufacturerId);
        }

        return manufacturer;
    }


    // =========================================================
    // Approved Prices
    // =========================================================

    public Result<Created> AddApprovedPrice(
        decimal price,
        string pricePerDescription,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        //if(_approvedPrices.Any(a=>
        
        //a.IssueDate>issueDate && !(expiryDate<a.IssueDate)
        //||
        //a.ExpiryDate<expiryDate && !(a.IssueDate<issueDate)
        
        //))
        //{
        //    return ProductBasicDataErrors.PriceExistsWithinRange;
        //}
        var priceResult =
            ApprovedPrice.Create(
                price,
                pricePerDescription,
                issueDate,
                expiryDate);

        if (priceResult.IsError)
        {
            return priceResult.TopError!;
        }

        _approvedPrices.Add(
            priceResult.Value);

        return Result.Created;
    }

    public Result<Updated> UpdateApprovedPrice(
        Guid approvedPriceId,
        decimal price,
        string pricePerDescription,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        var approvedPriceResult =
            GetApprovedPrice(
                approvedPriceId);

        if (approvedPriceResult.IsError)
        {
            return approvedPriceResult.TopError!;
        }

        return approvedPriceResult.Value.Update(
            price,
            pricePerDescription,
            issueDate,
            expiryDate);
    }

    public Result<Deleted> RemoveApprovedPrice(
        Guid approvedPriceId)
    {
        var priceResult =
            GetApprovedPrice(
                approvedPriceId);

        if (priceResult.IsError)
        {
            return priceResult.TopError!;
        }

        _approvedPrices.Remove(
            priceResult.Value);

        return Result.Deleted;
    }

    private Result<ApprovedPrice> GetApprovedPrice(
        Guid approvedPriceId)
    {
        var approvedPrice =
            _approvedPrices.FirstOrDefault(x =>
                x.Id == approvedPriceId);

        if (approvedPrice is null)
        {
            return ProductBasicDataErrors
                .ApprovedPriceNotFound(
                    approvedPriceId);
        }

        return approvedPrice;
    }


    // =========================================================
    // Approved Packs
    // =========================================================

    public Result<Created> AddApprovedPack(
    string description,
    DatedAttachment? approval)
    {
        var packResult =
            ApprovedPack.Create(
                description,
                approval);

        if (packResult.IsError)
        {
            return packResult.TopError!;
        }

        _approvedPacks.Add(
            packResult.Value);

        return Result.Created;
    }

    public Result<Updated> UpdateApprovedPack(
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

    public Result<Deleted> RemoveApprovedPack(
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
            _approvedPacks.FirstOrDefault(x =>
                x.Id == approvedPackId);

        if (pack is null)
        {
            return ProductBasicDataErrors
                .ApprovedPackNotFound(
                    approvedPackId);
        }

        return pack;
    }


    // =========================================================
    // PV
    // =========================================================

    public Result<Created> AddPV(
    string description,
    DateOnly deadline,
    string? notes)
    {
        if (string.IsNullOrWhiteSpace(
                description))
        {
            return ProductBasicDataErrors
                .PVDescriptionRequired;
        }


        var nextVersion =
            _pvHistory.Count == 0
                ? 1
                : _pvHistory.Max(
                    x => x.VersionNumber) + 1;


        var value =
            new FulfillmentDeadline(
                description.Trim(),
                deadline,
                notes);


        var historyItem =
            new PVDeadlineHistoryItem(
                nextVersion,
                value);


        _pvHistory.Add(
            historyItem);


        return Result.Created;
    }
    public Result<Updated> FulfillPV(
    DateOnly fulfillmentDate)
    {
        var current =
            CurrentPV;


        if (current is null)
        {
            return ProductBasicDataErrors
                .PVDeadlineNotDefined;
        }


        return current.Fulfill(
            fulfillmentDate);
    }

    public Result<Updated>
    ResetPVFulfillment()
    {
        var current =
            CurrentPV;


        if (current is null)
        {
            return ProductBasicDataErrors
                .PVDeadlineNotDefined;
        }


        return current
            .ResetFulfillment();
    }

    // =========================================================
    // Decree 600
    // =========================================================

    public Result<Created> AddDecree600(
        string description,
        DateOnly deadline,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(
                description))
        {
            return ProductBasicDataErrors
                .Decree600DescriptionRequired;
        }


        var nextVersion =
            _decree600History.Count == 0
                ? 1
                : _decree600History.Max(
                    x => x.VersionNumber) + 1;


        var value =
            new FulfillmentDeadline(
                description.Trim(),
                deadline,
                notes);


        var historyItem =
            new Decree600DeadlineHistoryItem(
                nextVersion,
                value);


        _decree600History.Add(
            historyItem);


        return Result.Created;
    }

    public Result<Updated> FulfillDecree600(
    DateOnly fulfillmentDate)
    {
        var current =
            CurrentDecree600;


        if (current is null)
        {
            return ProductBasicDataErrors
                .Decree600DeadlineNotDefined;
        }


        return current.Fulfill(
            fulfillmentDate);
    }


    public Result<Updated>
    ResetDecree600Fulfillment()
    {
        var current =
            CurrentDecree600;


        if (current is null)
        {
            return ProductBasicDataErrors
                .Decree600DeadlineNotDefined;
        }


        return current
            .ResetFulfillment();
    }

    public Result<Updated> UpdateActiveIngredientStorageCondition(
    Guid productActiveIngredientId,
    Guid? storageConditionId)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .UpdateStorageCondition(
                storageConditionId);
    }
    public Result<Updated> UpdateActiveIngredientPhysicalDescription(
    Guid productActiveIngredientId,
    string? physicalDescription)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .UpdatePhysicalDescription(
                physicalDescription);
    }
    public Result<Updated> SetActiveIngredientShelfLife(
    Guid productActiveIngredientId,
    int numberOfMonths,
    DateOnly approvalDate,
    IEnumerable<AttachmentReference> attachments)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId
            );

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .SetShelfLife(
                numberOfMonths,
                approvalDate,
                attachments
            );
    }

    public Result<Created>
    AddActiveIngredientShelfLifeAttachment(
        Guid productActiveIngredientId,
        AttachmentReference attachment)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId
            );

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .AddShelfLifeAttachment(
                attachment
            );
    }


    public Result<Deleted>
        RemoveActiveIngredientShelfLifeAttachment(
            Guid productActiveIngredientId,
            Guid attachmentId)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId
            );

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .RemoveShelfLifeAttachment(
                attachmentId
            );
    }

    public Result<Updated> UpdateActiveIngredientApprovedPack(
    Guid productActiveIngredientId,
    Guid approvedPackId,
    string description,
    DatedAttachment? approval)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .UpdateApprovedPack(
                approvedPackId,
                description,
                approval);
    }
    public Result<Deleted> RemoveActiveIngredientApprovedPack(
    Guid productActiveIngredientId,
    Guid approvedPackId)
    {
        var ingredientResult =
            GetActiveIngredient(
                productActiveIngredientId);

        if (ingredientResult.IsError)
        {
            return ingredientResult.TopError!;
        }

        return ingredientResult.Value
            .RemoveApprovedPack(
                approvedPackId);
    }
    public Result<Updated> UpdateTradeName(
    string? tradeName)
    {
        TradeName =
            string.IsNullOrWhiteSpace(tradeName)
                ? null
                : tradeName.Trim();

        return Result.Updated;
    }



}