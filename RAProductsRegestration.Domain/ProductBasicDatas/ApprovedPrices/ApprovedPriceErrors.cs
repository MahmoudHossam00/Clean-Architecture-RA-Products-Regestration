using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedPrices;

public static class ApprovedPriceErrors
{
    public static Error InvalidPrice =>
        Error.Validation(
            "ApprovedPrice.Price.Invalid",
            "Approved price cannot be negative.");

    public static Error InvalidValidityPeriod =>
        Error.Validation(
            "ApprovedPrice.ValidityPeriod.Invalid",
            "Expiry date cannot be before issue date.");
}