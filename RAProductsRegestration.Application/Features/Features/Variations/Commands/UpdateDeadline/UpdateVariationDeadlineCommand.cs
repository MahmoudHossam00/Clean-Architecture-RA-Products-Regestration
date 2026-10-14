using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.UpdateDeadline
{
    public sealed record UpdateVariationDeadlineCommand(
        Guid VariationId,
        DateOnly Deadline,
        string? DeadlineNote)
        : ICommand<Updated>;
}
