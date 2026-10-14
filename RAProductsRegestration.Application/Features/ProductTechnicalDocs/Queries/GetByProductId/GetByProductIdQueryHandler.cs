using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductsTechnicalDocs;
using System;
using System.Collections.Generic;
using System.Text;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Queries.GetByProductId
{
    internal sealed class Handler(
    IRepository<ProductTechnicalDocs> repository)
    : IQueryHandler<
        GetProductTechnicalDocsByProductIdQuery,
        ProductTechnicalDocsResponse>
    {
        public async Task<Result<ProductTechnicalDocsResponse>>
            Handle(
                GetProductTechnicalDocsByProductIdQuery request,
                CancellationToken ct)
        {
            var technicalDocs =
                await repository.FirstOrDefaultAsync(
                    x => x.ProductId == request.ProductId,
                    ct);

            if (technicalDocs is null)
            {
                return ProductTechnicalDocsErrors
                    .NotFoundForProduct(
                        request.ProductId);
            }


            return ProductTechnicalDocsMapper.Map(
                technicalDocs);
        }
    }
}
