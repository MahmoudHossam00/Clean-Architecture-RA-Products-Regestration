using RAProductsRegestration.Domain.ProductLicenses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence
{
    public interface IProductLicenseRepository:IRepository<ProductLicense>
    {
        Task<ProductLicense?>
        GetByIdWithRequirementsAsync(
            Guid id,
            CancellationToken ct = default);


        Task<ProductLicense?>
            GetByProductIdWithRequirementsAsync(
                Guid productId,
                CancellationToken ct = default);
    }
}
