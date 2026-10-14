using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.SetApproval
{
    public sealed record SetAppealApprovalCommand(
        Guid AppealId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
