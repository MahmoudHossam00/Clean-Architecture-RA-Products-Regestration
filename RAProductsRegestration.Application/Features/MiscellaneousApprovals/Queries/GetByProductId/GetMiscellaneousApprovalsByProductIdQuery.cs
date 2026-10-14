using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Queries.GetByProductId
{
    public sealed record GetMiscellaneousApprovalsByProductIdQuery(
        Guid ProductId)
        : IQuery<MiscellaneousApprovalResponse[]>;
}
