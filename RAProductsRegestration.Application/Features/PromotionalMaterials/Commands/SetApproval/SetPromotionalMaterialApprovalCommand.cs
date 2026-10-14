using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.SetApproval
{
public sealed 
        record SetPromotionalMaterialApprovalCommand(
    Guid PromotionalMaterialId,
    DatedAttachmentInput Approval,
    DateOnly ApprovalExpiryDate)
    : ICommand<Updated>;
}
