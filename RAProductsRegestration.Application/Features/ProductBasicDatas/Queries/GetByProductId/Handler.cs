using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Queries;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

internal sealed class Handler(
    IProductBasicDataReadService readService)
    : IQueryHandler<
        GetProductBasicDataByProductIdQuery,
        ProductBasicDataResponse>
{
    public async Task<Result<ProductBasicDataResponse>> Handle(
        GetProductBasicDataByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        var result =
            await readService.GetByProductIdAsync(
                request.ProductId,
                cancellationToken);

        if (result is null)
        {
            return Error.NotFound(
                "ProductBasicData.NotFound",
                $"Product basic data for product " +
                $"'{request.ProductId}' was not found.");
        }

        return result;
    }
}