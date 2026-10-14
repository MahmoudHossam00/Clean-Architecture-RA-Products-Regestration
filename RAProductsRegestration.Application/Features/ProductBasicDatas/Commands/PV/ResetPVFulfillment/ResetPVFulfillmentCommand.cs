using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.ResetPVFulfillment
{
    public sealed record ResetPVFulfillmentCommand(
    Guid ProductBasicDataId)
    : ICommand<Updated>;
}
