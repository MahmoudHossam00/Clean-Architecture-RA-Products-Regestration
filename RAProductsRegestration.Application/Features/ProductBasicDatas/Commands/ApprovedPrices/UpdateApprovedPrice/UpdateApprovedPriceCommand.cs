using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.UpdateApprovedPrice
{
    public sealed record UpdateApprovedPriceCommand(
    Guid ProductBasicDataId,
    Guid ApprovedPriceId,
    decimal Price,
    string PricePerDescription,
    DateOnly IssueDate,
    DateOnly ExpiryDate)
    : ICommand<Updated>;
}
