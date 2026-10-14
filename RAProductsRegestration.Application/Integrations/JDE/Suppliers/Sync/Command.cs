using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Integrations.JDE.Suppliers.Sync
{
    public sealed record SyncSuppliersCommand
        : ICommand<Success>;
}
