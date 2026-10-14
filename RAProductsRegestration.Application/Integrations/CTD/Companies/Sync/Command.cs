using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Integrations.CTD.Companies.Sync
{
    public sealed record SyncCompaniesCommand
    : ICommand<Success>;
}
