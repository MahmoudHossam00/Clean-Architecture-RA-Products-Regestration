using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Create
{

    public sealed record CreateMiscellaneousApprovalCommand(
        Guid ProductId,
        Guid FileTypeId,
        string Initiator,
        DateOnly SubmissionDate)
        : ICommand<Created>;
}
