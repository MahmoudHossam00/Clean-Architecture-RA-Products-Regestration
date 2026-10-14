using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Approve
{
    public sealed record ApproveApprovalRequirementCommand(
        Guid MiscellaneousApprovalId,

        Guid RequirementId,

        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
