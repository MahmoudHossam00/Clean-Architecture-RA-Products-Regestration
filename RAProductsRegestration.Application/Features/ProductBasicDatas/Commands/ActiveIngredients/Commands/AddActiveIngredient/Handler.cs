using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.ActiveIngredients;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ActiveIngredient> activeIngredientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddActiveIngredientCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddActiveIngredientCommand request,
            CancellationToken ct)
        {
            var exists =
                await activeIngredientRepository.AnyAsync(
                    x => x.Id == request.ActiveIngredientId,
                    ct);

            if (!exists)
            {
                return ProductBasicDataApplicationErrors
                    .ActiveIngredientNotFound(
                        request.ActiveIngredientId);
            }

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.AddActiveIngredient(
                        request.ActiveIngredientId,
                        request.Strength),
                    ct);
        }
    }
}
