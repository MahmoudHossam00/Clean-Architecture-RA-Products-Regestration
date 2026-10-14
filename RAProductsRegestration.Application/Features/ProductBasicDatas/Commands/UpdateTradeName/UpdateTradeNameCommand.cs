using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateTradeName
{
    public sealed record UpdateTradeNameCommand(
    Guid ProductBasicDataId,
    string? TradeName)
    : ICommand<Updated>;
}
