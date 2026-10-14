using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetLastApprovedLeaflet
{
    public sealed record SetLastApprovedLeafletCommand(
        Guid ProductLicenseId,
        DatedAttachmentInput Approval)
        : ICommand<Updated>;
}
