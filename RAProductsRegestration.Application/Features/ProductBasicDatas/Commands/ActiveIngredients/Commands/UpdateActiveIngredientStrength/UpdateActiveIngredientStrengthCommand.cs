using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength
{
    public sealed record UpdateActiveIngredientStrengthCommand(
    Guid ProductBasicDataId,
    Guid ProductActiveIngredientId,
    string Strength)
    : ICommand<Updated>;
}
