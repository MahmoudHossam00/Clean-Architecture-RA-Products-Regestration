using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll
{
    public sealed record GetStorageConditionsQuery(
    bool ActiveOnly = true)
    : IQuery<IReadOnlyCollection<
        StorageConditionLookupResponse>>;
}
