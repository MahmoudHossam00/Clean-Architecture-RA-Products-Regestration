using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Integrations.JDE.Suppliers
{
    internal sealed class SupplierRow
    {
        public long Code { get; set; }

        public string Name { get; set; } = null!;
    }
}
