using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals
{
    public interface IAppealRepository
    : IRepository<Appeal>
    {
        Task<Appeal?>
            GetByIdWithRequirementsAsync(
                Guid id,
                CancellationToken ct = default);


        Task<IReadOnlyList<Appeal>>
            GetByProductIdWithRequirementsAsync(
                Guid productId,
                CancellationToken ct = default);
    }
}
