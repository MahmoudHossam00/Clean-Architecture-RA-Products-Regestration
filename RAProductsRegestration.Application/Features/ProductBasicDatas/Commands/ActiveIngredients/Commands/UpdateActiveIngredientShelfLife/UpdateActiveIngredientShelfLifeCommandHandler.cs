using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Commands.ActiveIngredients.Commands
    .UpdateActiveIngredientShelfLife;

internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateActiveIngredientShelfLifeCommand,
        Updated>
{
    public Task<Result<Updated>> Handle(
        UpdateActiveIngredientShelfLifeCommand request,
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
                productBasicData =>
                    productBasicData
                        .SetActiveIngredientShelfLife(
                            request.ProductActiveIngredientId,
                            request.NumberOfMonths,
                            request.ApprovalDate,
                            attachments
                        ),
                ct
            );
    }
}