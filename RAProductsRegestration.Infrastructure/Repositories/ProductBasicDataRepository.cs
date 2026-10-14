using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.ProductBasicDatas;

namespace RAProductsRegestration.Infrastructure.Data.Repositories;

internal sealed class ProductBasicDataRepository(
    ApplicationDbContext context)
    : IProductBasicDataRepository
{
    public async Task<ProductBasicData?> GetAggregateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.ProductBasicData
            .Include(x => x.ProductActiveIngredients)
                .ThenInclude(x => x.ApprovedSuppliers)
            .Include(x => x.ProductActiveIngredients)
                .ThenInclude(x => x.ApprovedPacks)
            .Include(x => x.ApprovedPacks)
            .Include(x => x.ApprovedPrices)
            .Include(x => x.ApprovedManufacturers)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<ProductBasicData?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return await context.ProductBasicData
            .Include(x => x.ProductActiveIngredients)
                .ThenInclude(x => x.ApprovedSuppliers)
            .Include(x => x.ProductActiveIngredients)
                .ThenInclude(x => x.ApprovedPacks)
            .Include(x => x.ApprovedPacks)
            .Include(x => x.ApprovedPrices)
            .Include(x => x.ApprovedManufacturers)
            .FirstOrDefaultAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }

    public void Add(
        ProductBasicData productBasicData)
    {
        context.ProductBasicData.Add(
            productBasicData);
    }
}