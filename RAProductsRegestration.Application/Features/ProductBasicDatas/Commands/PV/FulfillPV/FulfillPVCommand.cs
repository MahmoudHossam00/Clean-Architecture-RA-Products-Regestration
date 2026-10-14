using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV
{
    public sealed record FulfillPVCommand(
    Guid ProductBasicDataId,
    DateOnly FulfillmentDate)
    : ICommand<Updated>;
}
