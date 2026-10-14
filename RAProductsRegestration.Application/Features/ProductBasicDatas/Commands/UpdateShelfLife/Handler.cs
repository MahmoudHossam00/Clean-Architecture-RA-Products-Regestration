using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Commands.UpdateShelfLife;


internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateShelfLifeCommand,
        Created>
{
    public Task<Result<Created>> Handle(
        UpdateShelfLifeCommand request,
        CancellationToken ct)
    {
        var attachments =
            request.Attachments
                .Select(x => x.ToDomain())
                .ToArray();

        return ProductBasicDataCommandExecutor
            .ExecuteAsync(
                request.ProductBasicDataId,
                repository,
                unitOfWork,
                x => x.AddShelfLife(
                    request.NumberOfMonths,
                    request.ApprovalDate,
                    attachments
                ),
                ct
            );
    }
}

