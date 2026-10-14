using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductLicenses.Requirements;

public static class LicenseRequirementErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "LicenseRequirement.NameRequired",
            "Requirement name is required.");


    public static readonly Error DepartmentRequired =
        Error.Validation(
            "LicenseRequirement.DepartmentRequired",
            "Responsible department is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "LicenseRequirement.NotFound",
            $"License requirement '{id}' was not found.");
}