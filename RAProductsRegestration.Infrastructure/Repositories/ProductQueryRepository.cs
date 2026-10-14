using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.Products.Queries.GetAll;
using RAProductsRegestration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Repositories
{
    internal sealed class ProductQueryRepository(
    ApplicationDbContext context)
    : IProductQueryRepository
    {
        public async Task<(
            IReadOnlyList<ProductsLookupResponse> Items,
            int TotalCount)> GetFilteredAsync(
                string? filter,
                int pageNumber,
                int pageSize,
                CancellationToken ct = default)
        {
            var query =
                from product in context.Products.AsNoTracking()
                join company in context.Companies.AsNoTracking()
                    on product.CompanyId equals company.Id
                select new
                {
                    Product = product,
                    CompanyName = company.Name
                };
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim();

                query = query.Where(x =>
                    x.Product.ProductName.Contains(filter) ||
                    x.Product.ProductCode.Contains(filter) ||
                    x.CompanyName.Contains(filter));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.Product.ProductCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                                .Select(x =>
                    new ProductsLookupResponse(
                        x.Product.Id,
                        x.Product.CTDId,
                        x.Product.ProductCode,
                        x.Product.ProductName,
                        x.Product.CompanyId,
                        x.CompanyName))
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
