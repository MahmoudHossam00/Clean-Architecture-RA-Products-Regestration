using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;

//using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using RAProductsRegestration.Domain.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Create
{
    internal sealed class Handler(
    IRepository<Product> productRepository,
    IProductBasicDataRepository productBasicDataRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateProductBasicDataCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateProductBasicDataCommand request,
            CancellationToken cancellationToken)
        {
            var productExists =
                await productRepository.AnyAsync(
                    x => x.Id == request.ProductId,
                    cancellationToken);

            if (!productExists)
            {
                return ProductBasicDataApplicationErrors
                    .ProductNotFound(
                        request.ProductId);
            }

            var existing =
                await productBasicDataRepository
                    .GetByProductIdAsync(
                        request.ProductId,
                        cancellationToken);

            if (existing is not null)
            {
                return ProductBasicDataApplicationErrors
                    .AlreadyExistsForProduct(
                        request.ProductId);
            }

            var createResult =
                ProductBasicData.Create(
                    request.ProductId,
                    request.TradeName);

            if (createResult.IsError)
            {
                return createResult.TopError!;
            }

            productBasicDataRepository.Add(
                createResult.Value);

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result.Created;
        }
    }
}
