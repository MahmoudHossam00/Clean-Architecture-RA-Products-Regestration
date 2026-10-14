using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.Delete
{
    public sealed record DeleteApprovalRequirementCommand(
        Guid MiscellaneousApprovalId,
        Guid RequirementId)
        : ICommand<Deleted>;
}
