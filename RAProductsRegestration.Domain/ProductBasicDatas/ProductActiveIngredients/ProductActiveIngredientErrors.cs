using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients;

public static class ProductActiveIngredientErrors
{
    public static Error ActiveIngredientIdRequired =>
        Error.Validation(
            "ProductActiveIngredient.ActiveIngredientId.Required",
            "Active ingredient ID cannot be empty.");
    public static readonly Error ShelfLifeNotDefined =
    Error.NotFound(
        "ProductActiveIngredient.ShelfLifeNotDefined",
        "Shelf life has not been defined for this active ingredient."
    );
    public static Error StrengthRequired =>
        Error.Validation(
            "ProductActiveIngredient.Strength.Required",
            "Strength cannot be empty.");
    public static Error SupplierAlreadyExists(
    Guid supplierId) =>
    Error.Conflict(
        "ProductActiveIngredient.Supplier.AlreadyExists",
        $"Supplier '{supplierId}' is already approved for this active ingredient.");
    public static Error SupplierAlreadyExists(string supplierName) =>
        Error.Conflict(
            "ProductActiveIngredient.Supplier.AlreadyExists",
            $"Supplier '{supplierName}' is already approved for this active ingredient.");

    public static Error SupplierNotFound(Guid supplierId) =>
        Error.NotFound(
            "ProductActiveIngredient.Supplier.NotFound",
            $"Approved supplier '{supplierId}' was not found.");
    public static Error ApprovedPackNotFound(Guid approvedPackId) =>
    Error.NotFound(
        "ProductActiveIngredient.ApprovedPack.NotFound",
        $"Approved pack '{approvedPackId}' was not found.");
    public static Error ApprovedSupplierNotFound(Guid ApprovedSupplierId) =>
    Error.NotFound(
        "ProductActiveIngredient.ApprovedSupplierNotFound.NotFound",
        $"ApprovedSupplierNotFound '{ApprovedSupplierId}' was not found.");
}