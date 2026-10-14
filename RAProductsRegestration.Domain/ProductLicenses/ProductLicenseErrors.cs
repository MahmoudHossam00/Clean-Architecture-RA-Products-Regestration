using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductLicenses;

public static class ProductLicenseErrors
{
    public static readonly Error ProductIdRequired =
        Error.Validation(
            "ProductLicense.ProductIdRequired",
            "Product ID is required.");


    public static readonly Error LicenseCategoryRequired =
        Error.Validation(
            "ProductLicense.LicenseCategoryRequired",
            "License category is required.");


    public static readonly Error MarketingTypeRequired =
        Error.Validation(
            "ProductLicense.MarketingTypeRequired",
            "Marketing type is required.");


    public static readonly Error ValidityDateBeforeIssueDate =
        Error.Validation(
            "ProductLicense.InvalidValidityDate",
            "License validity date cannot be before the issue date.");


    public static readonly Error RegistrationNumberRequired =
        Error.Validation(
            "ProductLicense.RegistrationNumberRequired",
            "Registration number/year is required.");


    public static readonly Error InvalidRegistrationNumber =
        Error.Validation(
            "ProductLicense.InvalidRegistrationNumber",
            "Registration number/year must contain two numbers separated by '/'.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "ProductLicense.NotFound",
            $"Product license '{id}' was not found.");


    public static Error NotFoundForProduct(
        Guid productId) =>
        Error.NotFound(
            "ProductLicense.NotFoundForProduct",
            $"Product license was not found for product '{productId}'.");


    public static Error AlreadyExistsForProduct(
        Guid productId) =>
        Error.Conflict(
            "ProductLicense.AlreadyExistsForProduct",
            $"A product license already exists for product '{productId}'.");
}