using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.ResetDecree600Fulfillment
{
    public sealed record ResetDecree600FulfillmentCommand(
    Guid ProductBasicDataId)
    : ICommand<Updated>;
}
