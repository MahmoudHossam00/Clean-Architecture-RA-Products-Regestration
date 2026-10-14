using RAProductsRegestration.Application.Features.ActiveIngredients.Queries.GetAll;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence;

public interface IActiveIngredientQueryRepository
    {
        Task<(
            IReadOnlyList<ActiveIngredientLookupResponse> Items,
            int TotalCount)> GetFilteredAsync(
                string? filter,
                int pageNumber,
                int pageSize,
                CancellationToken ct = default);
    }
