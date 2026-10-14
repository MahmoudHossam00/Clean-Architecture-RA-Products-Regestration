using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas;

public static class ProductBasicDataErrors
{
    public static Error ProductIdRequired =>
        Error.Validation(
            "ProductBasicData.ProductId.Required",
            "Product ID cannot be empty.");

    public static Error ActiveIngredientIdRequired =>
        Error.Validation(
            "ProductBasicData.ActiveIngredient.IdRequired",
            "Active ingredient ID cannot be empty.");
    public static readonly Error ShelfLifeNotDefined =
    Error.NotFound(
        "ProductBasicData.ShelfLifeNotDefined",
        "Shelf life has not been defined for this product."
    );
    public static Error StrengthRequired =>
        Error.Validation(
            "ProductBasicData.ActiveIngredient.StrengthRequired",
            "Strength cannot be empty.");

    public static Error ActiveIngredientAlreadyExists(Guid activeIngredientId) =>
        Error.Conflict(
            "ProductBasicData.ActiveIngredient.AlreadyExists",
            $"Active ingredient '{activeIngredientId}' is already assigned to this product.");

    public static Error ActiveIngredientNotFound(Guid id) =>
        Error.NotFound(
            "ProductBasicData.ActiveIngredient.NotFound",
            $"Product active ingredient '{id}' was not found.");

    public static Error SupplierNameRequired =>
        Error.Validation(
            "ProductBasicData.Supplier.NameRequired",
            "Supplier name cannot be empty.");

    public static Error SupplierAlreadyExists(string supplierName) =>
        Error.Conflict(
            "ProductBasicData.Supplier.AlreadyExists",
            $"Supplier '{supplierName}' is already approved for this active ingredient.");

    public static Error ApprovedSupplierNotFound(Guid id) =>
        Error.NotFound(
            "ProductBasicData.Supplier.NotFound",
            $"Approved supplier '{id}' was not found.");

    public static Error ManufacturerNameRequired =>
        Error.Validation(
            "ProductBasicData.Manufacturer.NameRequired",
            "Manufacturer name cannot be empty.");

    public static Error ManufacturerAlreadyExists(string manufacturerName) =>
        Error.Conflict(
            "ProductBasicData.Manufacturer.AlreadyExists",
            $"Manufacturer '{manufacturerName}' is already added.");

    public static Error ApprovedManufacturerNotFound(Guid id) =>
        Error.NotFound(
            "ProductBasicData.Manufacturer.NotFound",
            $"Approved manufacturer '{id}' was not found.");

    public static Error ApprovedPackRequired =>
        Error.Validation(
            "ProductBasicData.ApprovedPack.Required",
            "Approved pack data is required.");

    public static Error ApprovedPackNotFound(Guid id) =>
        Error.NotFound(
            "ProductBasicData.ApprovedPack.NotFound",
            $"Approved pack '{id}' was not found.");

    public static Error InvalidPrice =>
        Error.Validation(
            "ProductBasicData.ApprovedPrice.InvalidPrice",
            "Approved price cannot be negative.");

    public static Error InvalidPriceValidity =>
        Error.Validation(
            "ProductBasicData.ApprovedPrice.InvalidValidity",
            "Expiry date cannot be before issue date.");

    public static Error ApprovedPriceNotFound(Guid id) =>
        Error.NotFound(
            "ProductBasicData.ApprovedPrice.NotFound",
            $"Approved price '{id}' was not found.");

    public static Result<Created> NotFound(Guid productBasicDataId) =>
        Error.NotFound(
            "ProductBasicData.NotFound",
            $"ProductBasicData with id '{productBasicDataId}' was not found.");

    public static Error PVDeadlineNotDefined =>
        Error.Conflict(
            "ProductBasicData.PV.DeadlineNotDefined",
            "PV deadline has not been defined.");

    public static Error Decree600DeadlineNotDefined =>
        Error.Conflict(
            "ProductBasicData.Decree600.DeadlineNotDefined",
            "Decree 600 deadline has not been defined.");

    public static readonly Error Decree600DescriptionRequired =
       Error.Validation(
           "ProductBasicData.Decree600.DescriptionRequired",
           "Decree 600 description is required.");
    public static readonly Error PVDescriptionRequired =
       Error.Validation(
           "ProductBasicData.PV.DescriptionRequired",
           "PV description is required.");
    public static readonly Error PriceExistsWithinRange =
       Error.Validation(
           "ProductBasicData.ApprovedPrice.PriceExistsWithinRange",
           "there is another price within the entered date range.");


}