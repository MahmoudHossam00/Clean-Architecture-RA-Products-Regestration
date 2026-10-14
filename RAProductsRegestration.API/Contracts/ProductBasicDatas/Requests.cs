using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.ProductBasicDatas;

public sealed record CreateProductBasicDataRequest(
    Guid ProductId,
    string? TradeName);

public sealed record UpdateTradeNameRequest(
    string? TradeName);

public sealed record UpdateDosageFormRequest(
    Guid? DosageFormId);

public sealed record UpdateStorageConditionRequest(
    Guid? StorageConditionId);

public sealed record UpdatePhysicalDescriptionRequest(
    string? PhysicalDescription);

public sealed record UpdateShelfLifeRequest(
    ShelfLifeInput ShelfLife);

public sealed record AddActiveIngredientRequest(
    Guid ActiveIngredientId,
    string Strength);

public sealed record UpdateStrengthRequest(
    string Strength);

public sealed record AddApprovedSupplierRequest(
    Guid SupplierId);

public sealed record ApprovedPackRequest(
    string Description,
    DatedAttachmentInput Approval);

public sealed record AddApprovedManufacturerRequest(
    string ManufacturerName,
    DatedAttachmentInput Approval);

public sealed record ApprovedPriceRequest(
    decimal Price,
    string PricePerDescription,
    DateOnly IssueDate,
    DateOnly ExpiryDate);

public sealed record FulfillmentDeadlineRequest(
    string Description,
    DateOnly Deadline,
    string? Notes);

public sealed record FulfillRequest(
    DateOnly FulfillmentDate);