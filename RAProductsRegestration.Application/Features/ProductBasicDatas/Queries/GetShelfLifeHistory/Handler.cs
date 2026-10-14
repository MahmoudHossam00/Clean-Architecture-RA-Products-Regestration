using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Queries;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetShelfLifeHistory
{
    internal sealed class Handler(
    IShelfLifeHistoryReadService readService)
    : IQueryHandler<
        GetShelfLifeHistoryQuery,
        ShelfLifeHistoryResponse[]>
    {
        public async Task<Result<ShelfLifeHistoryResponse[]>> Handle(
            GetShelfLifeHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await readService
                    .GetByProductBasicDataIdAsync(
                        request.ProductBasicDataId,
                        cancellationToken);


            if (result is null)
            {
                return Error.NotFound(
                    "ProductBasicData.NotFound",
                    $"Product basic data " +
                    $"'{request.ProductBasicDataId}' " +
                    $"was not found.");
            }


            return result;
        }
    }
}
