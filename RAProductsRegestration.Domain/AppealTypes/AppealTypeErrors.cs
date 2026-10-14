using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.AppealTypes;

public static class AppealTypeErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "AppealType.NameRequired",
            "Appeal type name is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "AppealType.NotFound",
            $"Appeal type '{id}' was not found.");


    public static Error AlreadyExists(
        string name) =>
        Error.Conflict(
            "AppealType.AlreadyExists",
            $"Appeal type '{name}' already exists.");
}