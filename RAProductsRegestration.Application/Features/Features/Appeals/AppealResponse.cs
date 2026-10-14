using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals
{

    public sealed record AppealResponse(
        Guid Id,
        Guid ProductId,

        string Initiator,

        LookupResponse AppealType,

        DateOnly AppealDate,

        DatedAttachmentResponse? Approval,

        string? Notes,

        AppealRequirementResponse[] Requirements);
}
