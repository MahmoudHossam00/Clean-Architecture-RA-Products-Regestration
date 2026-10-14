using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveActiveIngredient
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ProductBasicData> productbasicdataRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveActiveIngredientCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveActiveIngredientCommand request,
            CancellationToken ct)
        {
            var exists =
                await productbasicdataRepository.AnyAsync(
                    x => x.Id == request.ProductBasicDataId && x.ProductActiveIngredients.Any(p => p.Id == request.ProductActiveIngredientId),
                    ct);

            if (!exists)
            {
                return ProductBasicDataApplicationErrors
                    .NotFound(
                        request.ProductBasicDataId);
            }

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.RemoveActiveIngredient(
                    request.ProductActiveIngredientId
                    ),
                    ct);
        }
    }
}
