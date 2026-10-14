using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseCategory
{
    public sealed record UpdateLicenseCategoryCommand(
        Guid ProductLicenseId,
        Guid LicenseCategoryId)
        : ICommand<Updated>;
}
