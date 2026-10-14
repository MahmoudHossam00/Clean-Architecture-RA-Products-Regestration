using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedPacks;

public static class ApprovedPackErrors
{
    public static Error ApprovalRequired =>
        Error.Validation(
            "ApprovedPack.Approval.Required",
            "Approval date and attachment are required.");
    public static readonly Error DescriptionRequired =
        Error.Validation(
            "ApprovedPack.DescriptionRequired",
            "Approved pack description is required.");

}