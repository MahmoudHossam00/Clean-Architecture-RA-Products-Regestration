using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.LicenseCategories;

public static class LicenseCategoryErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "LicenseCategory.NameRequired",
            "License category name is required.");

    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "LicenseCategory.NotFound",
            $"License category '{id}' was not found.");

    public static Error AlreadyExists(
        string name) =>
        Error.Conflict(
            "LicenseCategory.AlreadyExists",
            $"License category '{name}' already exists.");
}