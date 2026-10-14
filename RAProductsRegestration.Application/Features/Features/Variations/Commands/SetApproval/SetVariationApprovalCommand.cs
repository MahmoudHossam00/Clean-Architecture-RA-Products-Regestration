using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.SetApproval
{
    public sealed record SetVariationApprovalCommand(
        Guid VariationId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
