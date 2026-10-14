using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Create
{
    public sealed record CreateProductBasicDataCommand(
    Guid ProductId,
    string? TradeName)
    : ICommand<Created>;
}
