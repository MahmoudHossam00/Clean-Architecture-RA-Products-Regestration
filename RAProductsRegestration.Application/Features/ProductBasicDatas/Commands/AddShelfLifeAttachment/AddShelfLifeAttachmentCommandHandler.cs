using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.AddShelfLifeAttachment
{
    internal sealed class Handler(
        IProductBasicDataRepository repository,
        IUnitOfWork unitOfWork)
        : ICommandHandler<
            AddShelfLifeAttachmentCommand,
            Created>
    {
        public Task<Result<Created>> Handle(
            AddShelfLifeAttachmentCommand request,
            CancellationToken ct)
        {
            return ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    productBasicData =>
                        productBasicData
                            .AddShelfLifeAttachment(
                                request.Attachment.ToDomain()
                            ),
                    ct
                );
        }
    }
}
