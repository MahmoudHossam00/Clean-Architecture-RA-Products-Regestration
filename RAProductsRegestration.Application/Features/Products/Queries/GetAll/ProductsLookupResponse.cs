using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Products.Queries.GetAll
{
    public sealed record ProductsLookupResponse(
            Guid Id,
            int CTDId,
            string ProductCode,
            string ProductName,
            Guid CompanyId,
            string CompanyName)
    {
    }
}
