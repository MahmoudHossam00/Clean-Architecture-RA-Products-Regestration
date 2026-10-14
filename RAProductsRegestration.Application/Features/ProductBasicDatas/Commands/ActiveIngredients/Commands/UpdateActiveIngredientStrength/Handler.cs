using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.ActiveIngredients;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using RAProductsRegestration.Domain.ProductBasicDatas.ProductActiveIngredients;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ProductBasicData> productbasicdataRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateActiveIngredientStrengthCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateActiveIngredientStrengthCommand request,
            CancellationToken ct)
        {
            var exists =
                await productbasicdataRepository.AnyAsync(
                    x => x.Id == request.ProductBasicDataId && x.ProductActiveIngredients.Any(p=>p.Id==request.ProductActiveIngredientId),
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
                    x => x.UpdateActiveIngredientStrength(
                    request.ProductActiveIngredientId,
                    request.Strength),
                    ct);
        }
    }
}
