using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.UpdateType
{
    public sealed record UpdatePromotionalMaterialTypeCommand(
        Guid PromotionalMaterialId,
        Guid PromotionalMaterialTypeId)
        : ICommand<Updated>;
}
