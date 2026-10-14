using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.SetRequirementAttachment
{
    public sealed record SetAppealRequirementAttachmentCommand(
        Guid AppealId,
        Guid RequirementId,
        AttachmentReferenceInput Attachment)
        : ICommand<Updated>;
}
