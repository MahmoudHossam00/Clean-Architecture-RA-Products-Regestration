using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .ProductBasicDatas.Commands.ActiveIngredients.Commands
    .RemoveShelfLifeAttachment;

internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveActiveIngredientShelfLifeAttachmentCommand,
        Deleted>
{
    public Task<Result<Deleted>> Handle(
        RemoveActiveIngredientShelfLifeAttachmentCommand request,
        CancellationToken ct)
    {
        return ProductBasicDataCommandExecutor
            .ExecuteAsync(
                request.ProductBasicDataId,
                repository,
                unitOfWork,
                productBasicData =>
                    productBasicData
                        .RemoveActiveIngredientShelfLifeAttachment(
                            request.ProductActiveIngredientId,
                            request.AttachmentId
                        ),
                ct
            );
    }
}