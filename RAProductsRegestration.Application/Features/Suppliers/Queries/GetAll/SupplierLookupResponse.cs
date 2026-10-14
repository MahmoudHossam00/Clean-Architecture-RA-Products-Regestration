using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Suppliers.Queries.GetAll
{
    public sealed record SupplierLookupResponse(
    Guid Id,
    long JdeCode,
    string Name);
}
