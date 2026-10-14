using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Suppliers
{
    public static class ApprovedSupplierErrors
    {
        public static Error SupplierIdRequired =>
            Error.Validation(
                "ApprovedSupplier.SupplierId.Required",
                "Supplier ID cannot be empty.");
    }
}
