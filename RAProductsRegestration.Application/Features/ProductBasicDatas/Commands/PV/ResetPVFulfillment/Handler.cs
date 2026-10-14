using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.SetPV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.ResetPVFulfillment
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        ResetPVFulfillmentCommand,
        Updated>
    {
        public Task<Result<Updated>> Handle(
            ResetPVFulfillmentCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.ResetPVFulfillment(),
                     
                    ct);
        }
    }
}
