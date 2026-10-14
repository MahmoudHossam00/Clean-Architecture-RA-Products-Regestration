using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Queries.GetByProductId
{
    public sealed record GetProductTechnicalDocsByProductIdQuery(
        Guid ProductId)
        : IQuery<ProductTechnicalDocsResponse>;
}
