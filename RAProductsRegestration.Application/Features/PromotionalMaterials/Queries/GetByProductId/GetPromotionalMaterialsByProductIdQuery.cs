using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Queries.GetByProductId
{

    public sealed record GetPromotionalMaterialsByProductIdQuery(
        Guid ProductId)
        : IQuery<PromotionalMaterialResponse[]>;
}
