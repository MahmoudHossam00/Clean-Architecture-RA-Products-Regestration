using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations.Queries.GetByProductId
{
    public sealed record GetVariationsByProductIdQuery(
        Guid ProductId)
        : IQuery<VariationResponse[]>;
}
