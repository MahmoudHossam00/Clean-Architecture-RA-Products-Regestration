using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.ProductLicenses;
using RAProductsRegestration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Repositories
{
    internal class ProductLicenseRepository(ApplicationDbContext dbContext) : Repository<ProductLicense>(dbContext),IProductLicenseRepository
    {
        // =========================================================
        // Get By Id + Requirements
        // =========================================================

        public Task<ProductLicense?>
            GetByIdWithRequirementsAsync(
                Guid id,
                CancellationToken ct = default)
        {
            return dbContext
                .ProductLicenses
                .Include(
                    x => x.Requirements)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    ct);
        }


        // =========================================================
        // Get By Product Id + Requirements
        // =========================================================

        public Task<ProductLicense?>
            GetByProductIdWithRequirementsAsync(
                Guid productId,
                CancellationToken ct = default)
        {
            return dbContext
                .ProductLicenses
                .Include(
                    x => x.Requirements)
                .FirstOrDefaultAsync(
                    x =>
                        x.ProductId ==
                        productId,
                    ct);
        }
    }
}
