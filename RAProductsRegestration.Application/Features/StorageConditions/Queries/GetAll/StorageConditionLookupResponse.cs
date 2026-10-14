using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll
{
    public sealed record StorageConditionLookupResponse(
    Guid Id,
    string Name);
}
