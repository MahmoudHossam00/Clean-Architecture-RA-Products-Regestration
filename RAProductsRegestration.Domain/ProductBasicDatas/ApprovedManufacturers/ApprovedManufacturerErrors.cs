using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedManufacturers;

public static class ApprovedManufacturerErrors
{
    public static Error NameRequired =>
        Error.Validation(
            "ApprovedManufacturer.Name.Required",
            "Manufacturer name cannot be empty.");
}