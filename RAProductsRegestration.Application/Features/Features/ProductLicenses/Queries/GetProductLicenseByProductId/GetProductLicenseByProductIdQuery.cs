using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Queries.GetProductLicenseByProductId
{
    public sealed record GetProductLicenseByProductIdQuery(
        Guid ProductId)
        : IQuery<ProductLicenseResponse>;
}
