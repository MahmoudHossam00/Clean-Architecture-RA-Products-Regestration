using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.AddRequirement
{

    public sealed record AddLicenseRequirementCommand(
        Guid ProductLicenseId,
        string Name,
        DateOnly Deadline,
        Guid ResponsibleDepartmentId,
        AttachmentReferenceInput? RequirementAttachment,
        string? Notes)
        : ICommand<Created>;
}
