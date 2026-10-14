using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials
{
    public sealed record PromotionalMaterialResponse(
        Guid Id,
        Guid ProductId,
        LookupResponse Type,
        AttachmentResponse MaterialFile,
        DatedAttachmentResponse? Approval,
        DateOnly? ApprovalExpiryDate);
}
