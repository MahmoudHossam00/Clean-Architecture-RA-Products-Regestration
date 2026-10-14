using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models.IntegrationModels.CTD
{
    public sealed record CTDProduct(
    int CTDId,
    string ProductCode,
    string ProductName,
    string CompanyCode);
}
