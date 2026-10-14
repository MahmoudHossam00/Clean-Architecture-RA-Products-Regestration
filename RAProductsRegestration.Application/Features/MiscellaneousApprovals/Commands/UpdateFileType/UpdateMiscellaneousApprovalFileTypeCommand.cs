using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.UpdateFileType
{
    public sealed record UpdateMiscellaneousApprovalFileTypeCommand(
        Guid MiscellaneousApprovalId,
        Guid FileTypeId)
        : ICommand<Updated>;
}
