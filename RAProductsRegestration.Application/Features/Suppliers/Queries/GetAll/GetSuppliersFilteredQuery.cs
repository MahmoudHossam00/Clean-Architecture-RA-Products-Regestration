using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Suppliers.Queries.GetAll
{

    public sealed record GetSuppliersFilteredQuery(
        string? Filter,
        int PageNum = 1)
        : IQuery<
            PaginatedList<SupplierLookupResponse>>;
}
