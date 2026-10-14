using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Integrations.CTD
{
    internal static class CTDIntegrationErrors
    {
        internal static Error CompanyNotFound(
            string companyCode) =>
            Error.NotFound(
                "CTD.Company.NotFound",
                $"Company with CTD code '{companyCode}' was not found locally.");
    }
}
