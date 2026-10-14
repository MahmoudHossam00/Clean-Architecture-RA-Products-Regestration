using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Appeals;

public static class AppealErrors
{
    public static readonly Error ProductIdRequired =
        Error.Validation(
            "Appeal.ProductIdRequired",
            "Product ID is required.");


    public static readonly Error InitiatorRequired =
        Error.Validation(
            "Appeal.InitiatorRequired",
            "Appeal initiator is required.");


    public static readonly Error AppealTypeRequired =
        Error.Validation(
            "Appeal.AppealTypeRequired",
            "Appeal type is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "Appeal.NotFound",
            $"Appeal '{id}' was not found.");
}