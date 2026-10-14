using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Variations;

public static class VariationErrors
{
    public static readonly Error ProductIdRequired =
        Error.Validation(
            "Variation.ProductIdRequired",
            "Product ID is required.");


    public static readonly Error InitiatorRequired =
        Error.Validation(
            "Variation.InitiatorRequired",
            "Variation initiator is required.");


    public static readonly Error CauseRequired =
        Error.Validation(
            "Variation.CauseRequired",
            "Variation cause is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "Variation.NotFound",
            $"Variation '{id}' was not found.");
}