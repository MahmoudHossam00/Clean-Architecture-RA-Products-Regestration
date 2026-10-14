using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients
    .ApprovedSuppliers;

public static class ApprovedSupplierErrors
{
    public static Error NameRequired =>
        Error.Validation(
            "ApprovedSupplier.Name.Required",
            "Supplier name cannot be empty.");
    public static Error SupplierIdRequired =>
        Error.Validation(
            "ApprovedSupplier.Id.Required",
            "Supplier Id cannot be empty.");
}