using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientPhysicalDescription
{
    public sealed record
    UpdateActiveIngredientPhysicalDescriptionCommand(
        Guid ProductBasicDataId,
        Guid ProductActiveIngredientId,
        string? PhysicalDescription)
    : ICommand<Updated>;
}
