using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetLastApprovedLayout
{
    public sealed record SetLastApprovedLayoutCommand(
        Guid ProductLicenseId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
