using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations
{
    public sealed record VariationRequirementResponse(
     Guid Id,
     string Name,
     DateOnly Deadline,
     LookupResponse ResponsibleDepartment,
     AttachmentResponse? RequirementAttachment,
     DatedAttachmentResponse? Approval,
     string? Notes);
}
