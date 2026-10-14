using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetShelfLifeHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Queries
{
    public interface IShelfLifeHistoryReadService
    {
        Task<ShelfLifeHistoryResponse[]?>
            GetByProductBasicDataIdAsync(
                Guid productBasicDataId,
                CancellationToken cancellationToken = default);
    }
}
