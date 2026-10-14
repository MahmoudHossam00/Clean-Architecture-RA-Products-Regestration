using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.Update
{
    public sealed record UpdateApprovalRequirementCommand(
        Guid MiscellaneousApprovalId,

        Guid RequirementId,

        string Name,

        DateOnly Deadline,

        Guid ResponsibleDepartmentId,

        string? Notes)
        : ICommand<Updated>;
}
