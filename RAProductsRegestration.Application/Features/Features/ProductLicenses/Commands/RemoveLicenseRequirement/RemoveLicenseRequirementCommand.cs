using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.RemoveLicenseRequirement
{
    public sealed record RemoveLicenseRequirementCommand(
        Guid ProductLicenseId,
        Guid RequirementId)
        : ICommand<Deleted>;
}
