using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.ResetPVFulfillment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
    FulfillPVCommand,
    Updated>
    {
        public Task<Result<Updated>> Handle(
            FulfillPVCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.FulfillPV(request.FulfillmentDate),

                    ct);
        }
    }
}
