using RAProductsRegestration
    .Application.Common.Constants;
using RAProductsRegestration
    .Application.Common.Interfaces.Messaging;
using RAProductsRegestration
    .Application.Common.Interfaces.Persistence;
using RAProductsRegestration
    .Application.Common.Models;
using RAProductsRegestration
    .Domain.Common.Results;

namespace RAProductsRegestration
    .Application
    .Features
    .Suppliers
    .Queries
    .GetAll;

internal sealed class Handler(
    ISupplierQueryRepository repository)
    : IQueryHandler<
        GetSuppliersFilteredQuery,
        PaginatedList<SupplierLookupResponse>>
{
    public async Task<
        Result<PaginatedList<SupplierLookupResponse>>>
        Handle(
            GetSuppliersFilteredQuery request,
            CancellationToken ct)
    {
        var pageSize =
            PaginationConstants.DefaultPageSize;

        var pageNumber =
            request.PageNum < 1
                ? 1
                : request.PageNum;

        var result =
            await repository.GetFilteredAsync(
                request.Filter,
                pageNumber,
                pageSize,
                ct);

        return new PaginatedList<SupplierLookupResponse>
        {
            PageNumber =
                pageNumber,

            PageSize =
                pageSize,

            TotalCount =
                result.TotalCount,

            TotalPages =
                (int)Math.Ceiling(
                    result.TotalCount /
                    (double)pageSize),

            Items =
                result.Items
        };
    }
}