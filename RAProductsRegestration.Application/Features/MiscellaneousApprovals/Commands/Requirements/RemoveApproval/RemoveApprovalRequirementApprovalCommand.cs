using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.RemoveApproval
{
    public sealed record RemoveApprovalRequirementApprovalCommand(
        Guid MiscellaneousApprovalId,
        Guid RequirementId)
        : ICommand<Updated>;
}
