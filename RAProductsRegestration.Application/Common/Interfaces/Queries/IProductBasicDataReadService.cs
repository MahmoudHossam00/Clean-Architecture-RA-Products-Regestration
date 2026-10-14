using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

namespace RAProductsRegestration.Application.Common.Interfaces.Queries;

public interface IProductBasicDataReadService
{
    Task<ProductBasicDataResponse?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}