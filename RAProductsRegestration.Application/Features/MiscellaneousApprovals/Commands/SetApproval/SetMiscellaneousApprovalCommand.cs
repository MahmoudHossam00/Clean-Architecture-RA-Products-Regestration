using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.SetApproval
{
    public sealed record SetMiscellaneousApprovalCommand(
        Guid MiscellaneousApprovalId,
        DateOnly ApprovalDate,
        DateOnly ApprovalExpiryDate)
        : ICommand<Updated>;
}
