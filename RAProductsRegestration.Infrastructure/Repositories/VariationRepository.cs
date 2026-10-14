using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Features
    .Features.Variations;
using RAProductsRegestration.Domain.Variations;
using RAProductsRegestration.Infrastructure.Data;

namespace RAProductsRegestration.Infrastructure.Repositories;

internal sealed class VariationRepository(
    ApplicationDbContext dbContext)
    : Repository<Variation>(dbContext),
      IVariationRepository
{
    // =========================================================
    // Get One + Requirements
    // =========================================================

    public Task<Variation?>
        GetByIdWithRequirementsAsync(
            Guid id,
            CancellationToken ct = default)
    {
        return dbContext
            .Variations
            .Include(
                x => x.Requirements)
            .FirstOrDefaultAsync(
                x =>
                    x.Id == id,
                ct);
    }


    // =========================================================
    // Get Product Variations + Requirements
    // =========================================================

    public async Task<IReadOnlyList<Variation>>
        GetByProductIdWithRequirementsAsync(
            Guid productId,
            CancellationToken ct = default)
    {
        return await dbContext
            .Variations
            .Include(
                x => x.Requirements)
            .Where(
                x =>
                    x.ProductId ==
                    productId)
            .OrderBy(
                x => x.Deadline)
            .ToListAsync(
                ct);
    }
}