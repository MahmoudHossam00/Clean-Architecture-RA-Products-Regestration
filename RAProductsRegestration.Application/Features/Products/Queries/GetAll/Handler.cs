using RAProductsRegestration.Application.Common.Constants;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Products.Queries.GetAll
{
    internal sealed class Handler(
    IProductQueryRepository repository)
    : IQueryHandler<
        GetProductsFilteredQuery,
        PaginatedList<ProductsLookupResponse>>
    {
        public async Task<Result<PaginatedList<ProductsLookupResponse>>> Handle(
            GetProductsFilteredQuery request,
            CancellationToken ct)
        {
            var pageSize = PaginationConstants.DefaultPageSize;

            var pageNumber = request.pagenum < 1
                ? 1
                : request.pagenum;

            var result = await repository.GetFilteredAsync(
                request.filter,
                pageNumber,
                pageSize,
                ct);

            return new PaginatedList<ProductsLookupResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,

                TotalCount = result.TotalCount,

                TotalPages = (int)Math.Ceiling(
                    result.TotalCount / (double)pageSize),

                Items = result.Items
            };
        }
    }
}