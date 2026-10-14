using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.DeactivateApprovedManufacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.RemoveApprovedManfacturer
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    //IRepository<ActiveIngredient> activeIngredientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveApprovedManufacturerCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveApprovedManufacturerCommand request,
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
                    x => x.RemoveApprovedManufacturer(
                        request.ApprovedManufacturerId
                        ),
                    ct);
        }
    }
}
