using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.SetRequirementApproval
{
    public sealed record SetVariationRequirementApprovalCommand(
        Guid VariationId,
        Guid RequirementId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
