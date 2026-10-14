using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateRegistrationNumber
{
    public sealed record UpdateRegistrationNumberCommand(
        Guid ProductLicenseId,
        string RegistrationNumberYear)
        : ICommand<Updated>;
}
