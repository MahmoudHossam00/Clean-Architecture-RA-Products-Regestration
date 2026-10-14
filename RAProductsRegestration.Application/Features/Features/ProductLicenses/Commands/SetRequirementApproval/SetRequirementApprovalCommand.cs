using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetRequirementApproval
{
    public sealed record SetRequirementApprovalCommand(
        Guid ProductLicenseId,
        Guid RequirementId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
