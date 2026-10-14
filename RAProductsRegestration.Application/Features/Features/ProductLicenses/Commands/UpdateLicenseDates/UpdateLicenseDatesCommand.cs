using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseDates
{
    public sealed record UpdateLicenseDatesCommand(
        Guid ProductLicenseId,
        DateOnly IssueDate,
        DateOnly ValidityDate)
        : ICommand<Updated>;
}
