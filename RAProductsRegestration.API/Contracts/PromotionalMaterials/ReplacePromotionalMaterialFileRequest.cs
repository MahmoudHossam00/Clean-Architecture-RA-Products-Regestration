using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.PromotionalMaterials
{
    public sealed record ReplacePromotionalMaterialFileRequest(
        AttachmentReferenceInput MaterialFile);
}
