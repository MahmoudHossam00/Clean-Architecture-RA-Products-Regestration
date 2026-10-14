using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.SetRequirementAttachment
{
    public sealed record SetVariationRequirementAttachmentCommand(
        Guid VariationId,
        Guid RequirementId,
        AttachmentReferenceInput Attachment)
        : ICommand<Updated>;
}
