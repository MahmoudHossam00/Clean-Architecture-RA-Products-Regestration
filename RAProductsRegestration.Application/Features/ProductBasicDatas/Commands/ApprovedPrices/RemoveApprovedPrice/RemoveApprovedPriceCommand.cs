using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.RemoveApprovedPrice
{
    public sealed record RemoveApprovedPriceCommand(
    Guid ProductBasicDataId,
    Guid ApprovedPriceId)
    : ICommand<Deleted>;
}
