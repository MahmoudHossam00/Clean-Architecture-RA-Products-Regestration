using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations
{
    public sealed record VariationResponse(
        Guid Id,
        Guid ProductId,
        string Initiator,
        string Cause,
        DateOnly Deadline,
        string? DeadlineNote,
        DatedAttachmentResponse? Approval,
        IReadOnlyCollection<VariationRequirementResponse>
            Requirements);
}
