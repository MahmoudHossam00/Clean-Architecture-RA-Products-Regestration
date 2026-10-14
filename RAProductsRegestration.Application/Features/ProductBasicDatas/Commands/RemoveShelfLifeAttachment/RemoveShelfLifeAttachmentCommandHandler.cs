using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.RemoveShelfLifeAttachment
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveShelfLifeAttachmentCommand,
        Deleted>
    {
        public Task<Result<Deleted>> Handle(
            RemoveShelfLifeAttachmentCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    productBasicData =>
                        productBasicData
                            .RemoveShelfLifeAttachment(
                                request.AttachmentId
                            ),
                    ct
                );
        }
    }
}
