using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.Add
{
    public sealed record AddApprovalRequirementCommand(
        Guid MiscellaneousApprovalId,

        string Name,

        DateOnly Deadline,

        Guid ResponsibleDepartmentId,

        AttachmentReferenceInput?
            RequirementAttachment,

        string? Notes)
        : ICommand<Created>;
}
