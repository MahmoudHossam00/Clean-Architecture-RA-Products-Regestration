using RAProductsRegestration.Application.Common.Models.IntegrationModels.CTD;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD
{
    public interface ICTDDataSource
    {
        Task<IReadOnlyCollection<CTDProduct>> GetProductsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<CTDActiveIngredient>> GetActiveIngredientsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<CTDCompany>> GetCompaniesAsync(
            CancellationToken cancellationToken = default);
    }
}
