
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.PromotionalMaterials;

public static class PromotionalMaterialErrors
{
    public static readonly Error ProductIdRequired =
        Error.Validation(
            "PromotionalMaterial.ProductIdRequired",
            "Product ID is required.");


    public static readonly Error PromotionalMaterialTypeRequired =
        Error.Validation(
            "PromotionalMaterial.PromotionalMaterialTypeRequired",
            "Promotional material type is required.");


    public static readonly Error ApprovalExpiryBeforeApprovalDate =
        Error.Validation(
            "PromotionalMaterial.ApprovalExpiryBeforeApprovalDate",
            "Approval expiry date cannot be before the approval date.");


    public static Error NotFound(
        Guid id)
    {
        return Error.NotFound(
            "PromotionalMaterial.NotFound",
            $"Promotional material '{id}' was not found.");
    }


    public static Error NotFoundForProduct(
        Guid id,
        Guid productId)
    {
        return Error.NotFound(
            "PromotionalMaterial.NotFoundForProduct",
            $"Promotional material '{id}' was not found for product '{productId}'.");
    }
}