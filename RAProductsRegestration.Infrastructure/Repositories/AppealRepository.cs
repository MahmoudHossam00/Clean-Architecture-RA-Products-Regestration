using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Features
    .Features.Appeals;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Infrastructure.Data;

namespace RAProductsRegestration.Infrastructure.Repositories;

internal sealed class AppealRepository(
    ApplicationDbContext dbContext)
    : Repository<Appeal>(dbContext),
      IAppealRepository
{
    // =========================================================
    // Get By Id + Requirements
    // =========================================================

    public Task<Appeal?>
        GetByIdWithRequirementsAsync(
            Guid id,
            CancellationToken ct = default)
    {
        return dbContext
            .Appeals
            .Include(
                x => x.Requirements)
            .FirstOrDefaultAsync(
                x =>
                    x.Id == id,
                ct);
    }


    // =========================================================
    // Get Product Appeals + Requirements
    // =========================================================

    public async Task<IReadOnlyList<Appeal>>
        GetByProductIdWithRequirementsAsync(
            Guid productId,
            CancellationToken ct = default)
    {
        return await dbContext
            .Appeals
            .Include(
                x => x.Requirements)
            .Where(
                x =>
                    x.ProductId ==
                    productId)
            .OrderBy(
                x => x.AppealDate)
            .ToListAsync(
                ct);
    }
}