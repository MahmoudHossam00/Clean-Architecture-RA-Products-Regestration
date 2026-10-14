using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.UpdateDetails
{
    public sealed record UpdateMiscellaneousApprovalDetailsCommand(
        Guid MiscellaneousApprovalId,
        string Initiator,
        DateOnly SubmissionDate)
        : ICommand<Updated>;
}
