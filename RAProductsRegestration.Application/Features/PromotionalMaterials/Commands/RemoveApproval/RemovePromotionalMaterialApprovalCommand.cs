using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.RemoveApproval
{
    public sealed record RemovePromotionalMaterialApprovalCommand(
        Guid PromotionalMaterialId)
        : ICommand<Updated>;
}
