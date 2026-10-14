using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredientApprovedPack
{
    public sealed record AddActiveIngredientApprovedPackCommand(
    Guid ProductBasicDataId,
    Guid ProductActiveIngredientId,
    string Description,
    DatedAttachmentInput Approval)
    : ICommand<Created>;
}
