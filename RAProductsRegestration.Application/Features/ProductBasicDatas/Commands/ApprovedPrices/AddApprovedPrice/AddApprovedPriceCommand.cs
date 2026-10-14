using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.AddApprovedPrice
{
    public sealed record AddApprovedPriceCommand(
    Guid ProductBasicDataId,
    decimal Price,
    string pricePerDescription,
    DateOnly IssueDate,
    DateOnly ExpiryDate)
    : ICommand<Created>;
}
