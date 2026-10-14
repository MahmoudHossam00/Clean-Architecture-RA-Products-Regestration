using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.FulfillDecree600;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.ResetDecree600Fulfillment
{
    internal sealed class Handler(
        IProductBasicDataRepository repository,
        IUnitOfWork unitOfWork)
        : ICommandHandler<
        ResetDecree600FulfillmentCommand,
        Updated>
    {
        public Task<Result<Updated>> Handle(
            ResetDecree600FulfillmentCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.ResetDecree600Fulfillment(),

                    ct);
        }
    }
}
