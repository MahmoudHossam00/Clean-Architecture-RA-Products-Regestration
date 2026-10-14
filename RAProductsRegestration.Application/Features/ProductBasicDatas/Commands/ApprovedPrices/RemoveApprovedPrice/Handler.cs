using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.UpdateApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.RemoveApprovedPrice
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<DosageForm> dosageFormRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveApprovedPriceCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveApprovedPriceCommand request,
            CancellationToken ct)
        {


            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                   x => x.RemoveApprovedPrice(
                    request.ApprovedPriceId),
                    ct);
        }
    }
}
