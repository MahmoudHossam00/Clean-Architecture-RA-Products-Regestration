using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.AddRequirement
{
    public sealed record AddAppealRequirementCommand(
        Guid AppealId,
        string Name,
        DateOnly Deadline,
        Guid ResponsibleDepartmentId,
        AttachmentReferenceInput? RequirementAttachment,
        string? Notes)
        : ICommand<Created>;
}
