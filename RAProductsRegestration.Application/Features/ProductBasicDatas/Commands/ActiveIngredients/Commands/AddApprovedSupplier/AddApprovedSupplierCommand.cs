using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddApprovedSupplier
{
    public sealed record AddApprovedSupplierCommand(
    Guid ProductBasicDataId,
    Guid ProductActiveIngredientId,
    Guid SupplierId)
    : ICommand<Created>;
}
