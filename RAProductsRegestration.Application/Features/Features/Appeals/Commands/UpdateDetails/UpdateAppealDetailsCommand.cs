using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.UpdateDetails
{
    public sealed record UpdateAppealDetailsCommand(
        Guid AppealId,
        string Initiator,
        Guid AppealTypeId,
        DateOnly AppealDate,
        string? Notes)
        : ICommand<Updated>;
}
