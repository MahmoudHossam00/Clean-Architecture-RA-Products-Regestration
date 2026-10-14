using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseRequirement
{
    public sealed record UpdateLicenseRequirementCommand(
        Guid ProductLicenseId,
        Guid RequirementId,
        string Name,
        DateOnly Deadline,
        Guid ResponsibleDepartmentId,
        string? Notes)
        : ICommand<Updated>;

}
