using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.PromotionalMaterials
{
    public sealed record CreatePromotionalMaterialRequest(
        Guid ProductId,
        Guid PromotionalMaterialTypeId,
        AttachmentReferenceInput MaterialFile);
}
