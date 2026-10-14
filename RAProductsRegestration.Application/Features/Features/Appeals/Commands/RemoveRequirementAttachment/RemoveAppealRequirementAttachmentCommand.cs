using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.RemoveRequirementAttachment
{
    public sealed record RemoveAppealRequirementAttachmentCommand(
        Guid AppealId,
        Guid RequirementId)
        : ICommand<Updated>;
}
