using RAProductsRegestration.Application.Common.Interfaces.Messaging;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

public sealed record GetProductBasicDataByProductIdQuery(
    Guid ProductId)
    : IQuery<ProductBasicDataResponse>;