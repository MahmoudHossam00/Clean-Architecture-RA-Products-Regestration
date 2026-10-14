using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD;
using RAProductsRegestration.Application.Common.Models.IntegrationModels.CTD;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Integrations.CTD
{
    internal sealed class CTDDataSource(
    CTDReadDbContext context)
    : ICTDDataSource
    {
        public async Task<IReadOnlyCollection<CTDCompany>>
            GetCompaniesAsync(
                CancellationToken cancellationToken = default)
        {
            return await context.Companies
                .AsNoTracking()
                .Select(x => new CTDCompany(
                    x.Code,
                    x.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<CTDActiveIngredient>>
            GetActiveIngredientsAsync(
                CancellationToken cancellationToken = default)
        {
            return await context.APIs
                .AsNoTracking()
                .Select(x => new CTDActiveIngredient(
                    x.Id,
                    x.Code,
                    x.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<CTDProduct>>
            GetProductsAsync(
                CancellationToken cancellationToken = default)
        {
            return await context.Products
                .AsNoTracking()
                .Where(x=>x.CompanyCode!="")
                .Select(x => new CTDProduct(
                    x.Id,
                    x.ProductCode,
                    x.ProductName,
                    x.CompanyCode))
                .ToListAsync(cancellationToken);
        }
    }
}
