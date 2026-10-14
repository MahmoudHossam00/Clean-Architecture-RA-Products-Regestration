using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Common
{
    internal static class ProductBasicDataCommandExecutor
    {
        public static async Task<Result<T>> ExecuteAsync<T>(
            Guid productBasicDataId,
            IProductBasicDataRepository repository,
            IUnitOfWork unitOfWork,
            Func<ProductBasicData, Result<T>> operation,
            CancellationToken cancellationToken)
        {
            var productBasicData =
                await repository.GetAggregateAsync(
                    productBasicDataId,
                    cancellationToken);

            if (productBasicData is null)
            {
                return ProductBasicDataApplicationErrors
                    .NotFound(productBasicDataId);
            }

            var result =
                operation(productBasicData);

            if (result.IsError)
            {
                return result.TopError!;
            }

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            return result;
        }
    }
}
