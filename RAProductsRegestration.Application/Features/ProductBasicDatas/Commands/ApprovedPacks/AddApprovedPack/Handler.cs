using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedPacks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.AddApprovedPack
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    //IRepository<ApprovedPack> activeIngredientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovedPackCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovedPackCommand request,
            CancellationToken ct)
        {
            

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                   x => x.AddApprovedPack(
                    request.Description,
                    request.Approval.ToDomain()),
                    ct);
        }
    }
}
