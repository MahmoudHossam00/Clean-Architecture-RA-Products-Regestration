using RAProductsRegestration.API.Contracts.ProductLicenses;

namespace RAProductsRegestration.API.Contracts.PromotionalMaterials
{
    public sealed record SetPromotionalMaterialApprovalRequest(
        DatedAttachmentRequest Approval,
        DateOnly ApprovalExpiryDate);
}
