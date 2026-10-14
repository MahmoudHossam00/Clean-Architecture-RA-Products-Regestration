using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddShelfLifeAttachment
{

    public sealed record AddActiveIngredientShelfLifeAttachmentCommand(
        Guid ProductBasicDataId,
        Guid ProductActiveIngredientId,
        AttachmentReferenceInput Attachment)
        : ICommand<Created>;


}
