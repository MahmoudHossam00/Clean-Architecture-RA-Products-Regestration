using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.MarketingTypes.Queries.GetAll
{
    public sealed record GetMarketingTypesQuery(
        string? Search)
        : IQuery<IReadOnlyCollection<LookupResponse>>;
}
