using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.Create
{
    public sealed record CreateAppealCommand(
        Guid ProductId,
        string Initiator,
        Guid AppealTypeId,
        DateOnly AppealDate,
        string? Notes)
        : ICommand<Created>;
}
