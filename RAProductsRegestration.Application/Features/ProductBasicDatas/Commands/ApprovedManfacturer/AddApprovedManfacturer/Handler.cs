using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.AddApprovedManfacturer
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    //IRepository<ActiveIngredient> activeIngredientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovedManufacturerCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovedManufacturerCommand request,
            CancellationToken ct)
        {
            //var exists =
            //    await activeIngredientRepository.AnyAsync(
            //        x => x.Id == request.ActiveIngredientId,
            //        ct);

            //if (!exists)
            //{
            //    return ProductBasicDataApplicationErrors
            //        .ActiveIngredientNotFound(
            //            request.ActiveIngredientId);
            //}

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.AddApprovedManufacturer(
                        request.ManufacturerName,
                        request.Approval.ToDomain()),
                    ct);
        }
    }
}
