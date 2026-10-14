using RAProductsRegestration.Application.Features.Products.Queries.GetAll;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence;

public interface IProductQueryRepository
{
    Task<(IReadOnlyList<ProductsLookupResponse> Items, int TotalCount)>
        GetFilteredAsync(
            string? filter,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

}
