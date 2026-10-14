using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.FulfillDecree600
{
    public sealed record FulfillDecree600Command(
    Guid ProductBasicDataId,
    DateOnly FulfillmentDate)
    : ICommand<Updated>;
}
