using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.RemoveRequirement
{
    public sealed record RemoveVariationRequirementCommand(
        Guid VariationId,
        Guid RequirementId)
        : ICommand<Deleted>;
}
