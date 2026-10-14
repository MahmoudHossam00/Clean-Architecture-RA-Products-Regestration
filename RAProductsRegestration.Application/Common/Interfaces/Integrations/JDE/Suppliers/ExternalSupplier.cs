using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Integrations.JDE.Suppliers
{
    public sealed record ExternalSupplier(
     long Code,
     string Name);
}
