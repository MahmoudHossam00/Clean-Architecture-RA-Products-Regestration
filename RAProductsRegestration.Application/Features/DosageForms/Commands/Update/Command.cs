using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.DosageForms.Commands.Update
{
    public sealed record UpdateDosageFormCommand(
        Guid Id,
        string Name)
        : ICommand<Updated>;
}
