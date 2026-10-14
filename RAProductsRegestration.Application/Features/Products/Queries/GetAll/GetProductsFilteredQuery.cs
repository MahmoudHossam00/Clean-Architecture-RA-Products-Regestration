using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Products.Queries.GetAll
{
   
    public sealed record GetProductsFilteredQuery(
        int pagenum=1,
string? filter = null)
: IQuery<PaginatedList<ProductsLookupResponse>>;
}
