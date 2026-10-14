using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Variations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Variations
{
    public interface IVariationRepository
    : IRepository<Variation>
    {
        Task<Variation?>
            GetByIdWithRequirementsAsync(
                Guid id,
                CancellationToken ct = default);


        Task<IReadOnlyList<Variation>>
            GetByProductIdWithRequirementsAsync(
                Guid productId,
                CancellationToken ct = default);
    }
}
