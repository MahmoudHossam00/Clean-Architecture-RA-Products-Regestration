
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain
    .MiscellaneousApprovals.Requirements;

public static class ApprovalRequirementErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "ApprovalRequirement.NameRequired",
            "Approval requirement name is required.");


    public static readonly Error ResponsibleDepartmentRequired =
        Error.Validation(
            "ApprovalRequirement.ResponsibleDepartmentRequired",
            "Responsible department is required.");


    public static Error NotFound(
        Guid id)
    {
        return Error.NotFound(
            "ApprovalRequirement.NotFound",
            $"Approval requirement '{id}' was not found.");
    }
}