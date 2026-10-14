using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Commands.Decree600.FulfillDecree600;

internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        FulfillDecree600Command,
        Updated>
{
    public Task<Result<Updated>> Handle(
        FulfillDecree600Command request,
        CancellationToken ct)
    {
        return ProductBasicDataCommandExecutor
            .ExecuteAsync(
                request.ProductBasicDataId,
                repository,
                unitOfWork,

                basicData =>
                    basicData.FulfillDecree600(
                        request.FulfillmentDate),

                ct);
    }
}