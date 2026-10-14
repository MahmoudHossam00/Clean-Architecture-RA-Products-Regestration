using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.SetAttachment
{
    public sealed record SetApprovalRequirementAttachmentCommand(
        Guid MiscellaneousApprovalId,

        Guid RequirementId,

        AttachmentReferenceInput Attachment)
        : ICommand<Updated>;
}
