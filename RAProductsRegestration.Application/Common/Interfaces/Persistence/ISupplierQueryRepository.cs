using RAProductsRegestration.Application.Features.Suppliers.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence
{
    public interface ISupplierQueryRepository
    {
        Task<(
            IReadOnlyList<SupplierLookupResponse> Items,
            int TotalCount)>
            GetFilteredAsync(
                string? filter,
                int pageNumber,
                int pageSize,
                CancellationToken ct = default);
    }
}
