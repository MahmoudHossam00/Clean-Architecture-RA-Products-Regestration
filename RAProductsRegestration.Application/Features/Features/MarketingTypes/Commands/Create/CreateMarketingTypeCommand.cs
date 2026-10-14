using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.MarketingTypes.Commands.Create
{
    public sealed record CreateMarketingTypeCommand(
        string Name)
        : ICommand<Created>;
}
