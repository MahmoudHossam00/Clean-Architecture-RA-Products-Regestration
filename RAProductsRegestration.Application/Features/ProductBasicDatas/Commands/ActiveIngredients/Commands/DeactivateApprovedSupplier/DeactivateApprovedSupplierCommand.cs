using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.DeactivateApprovedSupplier
{
    public sealed record DeactivateApprovedSupplierCommand(
    Guid ProductBasicDataId,
    Guid ProductActiveIngredientId,
    Guid ApprovedSupplierId)
    : ICommand<Updated>;
}
