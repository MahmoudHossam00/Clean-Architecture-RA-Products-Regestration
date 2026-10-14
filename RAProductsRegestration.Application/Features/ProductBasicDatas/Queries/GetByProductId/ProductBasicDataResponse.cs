namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

public sealed record ProductBasicDataResponse(
    Guid Id,
    Guid ProductId,
    string ProductCode,
    string ProductName,
    string? TradeName,

    LookupResponse? DosageForm,
    LookupResponse? StorageCondition,

    string? PhysicalDescription,

    ShelfLifeResponse? ShelfLife,

    // =====================================================
    // PV
    // =====================================================

    FulfillmentDeadlineHistoryResponse? CurrentPV,

    FulfillmentDeadlineHistoryResponse[] PVHistory,

    // =====================================================
    // Decree 600
    // =====================================================

    FulfillmentDeadlineHistoryResponse? CurrentDecree600,

    FulfillmentDeadlineHistoryResponse[] Decree600History,

    // =====================================================
    // Active Ingredients
    // =====================================================

    IReadOnlyCollection<ProductActiveIngredientResponse>
        ActiveIngredients,

    // =====================================================
    // Approved Packs
    // =====================================================

    IReadOnlyCollection<ApprovedPackResponse>
        ApprovedPacks,

    // =====================================================
    // Approved Prices
    // =====================================================

    IReadOnlyCollection<ApprovedPriceResponse>
        ApprovedPrices,

    // =====================================================
    // Approved Manufacturers
    // =====================================================

    IReadOnlyCollection<ApprovedManufacturerResponse>
        ApprovedManufacturers);
public sealed record ShelfLifeResponse(
    int NumberOfMonths,
    DateOnly ApprovalDate,
    IReadOnlyCollection<AttachmentResponse> Attachments);
public sealed record LookupResponse(
    Guid Id,
    string Name);

public sealed record AttachmentResponse(
    Guid AttachmentId,
    string FileName,
    string? Notes);

public sealed record DatedAttachmentResponse(
    DateOnly Date,
    AttachmentResponse Attachment);

public sealed record FulfillmentDeadlineHistoryResponse(
    Guid Id,
    int VersionNumber,
    string Description,
    DateOnly Deadline,
    string? Notes,
    DateOnly? FulfillmentDate);

public sealed record ApprovedPriceResponse(
    Guid Id,
    decimal Price,
    string PricePerDescription,
    DateOnly IssueDate,
    DateOnly ExpiryDate);

public sealed record ApprovedPackResponse(
    Guid Id,
    string Description,
    DatedAttachmentResponse Approval);

public sealed record ApprovedManufacturerResponse(
    Guid Id,
    string ManufacturerName,
    bool IsActive,
    DatedAttachmentResponse Approval);

public sealed record ApprovedSupplierResponse(
    Guid Id,
    Guid SupplierId,
    long JdeCode,
    string SupplierName,
    bool IsActive);

public sealed record ProductActiveIngredientResponse(
    Guid Id,
    LookupResponse ActiveIngredient,
    string Strength,
    LookupResponse? StorageCondition,
    string? PhysicalDescription,
    ShelfLifeResponse? ShelfLife,
    IReadOnlyCollection<ApprovedSupplierResponse> ApprovedSuppliers,
    IReadOnlyCollection<ApprovedPackResponse> ApprovedPacks);