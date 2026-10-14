using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ActiveIngredients.Queries.GetAll
{
    public sealed record ActiveIngredientLookupResponse(
        Guid Id,
        int CTDId,
        string Code,
        string Name);
}
