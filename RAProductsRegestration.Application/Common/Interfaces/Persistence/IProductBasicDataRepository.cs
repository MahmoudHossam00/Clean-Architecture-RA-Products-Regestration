using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence
{
    public interface IProductBasicDataRepository
    {
        Task<ProductBasicData?> GetAggregateAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ProductBasicData?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);

        void Add(ProductBasicData productBasicData);
    }
}
