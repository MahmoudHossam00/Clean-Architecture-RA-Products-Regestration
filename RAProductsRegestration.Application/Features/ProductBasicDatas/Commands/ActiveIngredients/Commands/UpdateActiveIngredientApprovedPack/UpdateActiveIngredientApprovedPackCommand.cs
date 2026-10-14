using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientApprovedPack
{
    public sealed record UpdateActiveIngredientApprovedPackCommand(
    Guid ProductBasicDataId,
    Guid ProductActiveIngredientId,
    Guid ApprovedPackId,
    string Description,
    DatedAttachmentInput Approval)
    : ICommand<Updated>;
}
