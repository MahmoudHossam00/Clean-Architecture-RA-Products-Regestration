using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddShelfLifeAttachment
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddActiveIngredientShelfLifeAttachmentCommand,
        Created>
    {
        public Task<Result<Created>> Handle(
            AddActiveIngredientShelfLifeAttachmentCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    productBasicData =>
                        productBasicData
                            .AddActiveIngredientShelfLifeAttachment(
                                request.ProductActiveIngredientId,
                                request.Attachment.ToDomain()
                            ),
                    ct
                );
        }
    }
}
