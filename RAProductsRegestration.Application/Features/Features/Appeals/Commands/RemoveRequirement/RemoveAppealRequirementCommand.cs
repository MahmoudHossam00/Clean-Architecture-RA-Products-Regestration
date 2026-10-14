using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.RemoveRequirement
{
    public sealed record RemoveAppealRequirementCommand(
        Guid AppealId,
        Guid RequirementId)
        : ICommand<Deleted>;
}
