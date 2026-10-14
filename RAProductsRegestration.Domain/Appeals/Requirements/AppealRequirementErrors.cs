using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Appeals.Requirements;

public static class AppealRequirementErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "AppealRequirement.NameRequired",
            "Requirement name is required.");


    public static readonly Error DepartmentRequired =
        Error.Validation(
            "AppealRequirement.DepartmentRequired",
            "Responsible department is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "AppealRequirement.NotFound",
            $"Appeal requirement '{id}' was not found.");
}