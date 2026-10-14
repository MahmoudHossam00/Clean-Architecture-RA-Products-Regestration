using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientShelfLife;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using RAProductsRegestration.Domain.ProductBasicDatas.ApprovedPacks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.UpdateApprovedPack
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    //IRepository<ApprovedPack> approvedpacks,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateApprovedPackCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateApprovedPackCommand request,
            CancellationToken ct)
        {
           

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.UpdateApprovedPack(
                    request.ApprovedPackId,
                    request.Description,
                    request.Approval.ToDomain()),
                    ct);
        }
    }
}
