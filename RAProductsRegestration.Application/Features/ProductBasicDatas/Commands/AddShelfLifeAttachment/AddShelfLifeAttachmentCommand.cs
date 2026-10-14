using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.AddShelfLifeAttachment
{
    public sealed record AddShelfLifeAttachmentCommand(
        Guid ProductBasicDataId,
        AttachmentReferenceInput Attachment)
        : ICommand<Created>;
}
