using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.Create
{
    public sealed record CreateProductLicenseCommand(
        Guid ProductId,
        Guid LicenseCategoryId,
        DateOnly IssueDate,
        DateOnly ValidityDate,
        Guid MarketingTypeId,
        string RegistrationNumberYear)
        : ICommand<Created>;
}
