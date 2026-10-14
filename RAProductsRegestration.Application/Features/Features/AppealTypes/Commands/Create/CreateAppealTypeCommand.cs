using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.AppealTypes.Commands.Create
{
    public sealed record CreateAppealTypeCommand(
        string Name)
        : ICommand<Created>;
}
