using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientApprovedPack
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ProductBasicData> productbasicdataRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateActiveIngredientApprovedPackCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateActiveIngredientApprovedPackCommand request,
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
                    x => x.UpdateActiveIngredientApprovedPack(
             request.ProductActiveIngredientId,
             request.ApprovedPackId,
             request.Description,
             request.Approval.ToDomain()),
                    ct);
        }
    }
}
