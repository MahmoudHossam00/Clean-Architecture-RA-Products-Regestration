using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals
{
    public sealed record AppealRequirementResponse(
        Guid Id,
        string Name,
        DateOnly Deadline,
        LookupResponse ResponsibleDepartment,
        AttachmentResponse? RequirementAttachment,
        DatedAttachmentResponse? Approval,
        string? Notes);
}
