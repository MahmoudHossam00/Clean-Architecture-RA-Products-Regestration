
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;

public static class MiscellaneousApprovalFileTypeErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "MiscellaneousApprovalFileType.NameRequired",
            "Miscellaneous approval file type name is required.");


    public static Error NotFound(
        Guid id)
    {
        return Error.NotFound(
            "MiscellaneousApprovalFileType.NotFound",
            $"Miscellaneous approval file type '{id}' was not found.");
    }


    public static Error AlreadyExists(
        string name)
    {
        return Error.Conflict(
            "MiscellaneousApprovalFileType.AlreadyExists",
            $"Miscellaneous approval file type '{name}' already exists.");
    }
}