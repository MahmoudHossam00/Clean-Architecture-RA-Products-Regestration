using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Variations.Requirements;

public static class VariationRequirementErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "VariationRequirement.NameRequired",
            "Requirement name is required.");


    public static readonly Error DepartmentRequired =
        Error.Validation(
            "VariationRequirement.DepartmentRequired",
            "Responsible department is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "VariationRequirement.NotFound",
            $"Variation requirement '{id}' was not found.");
}