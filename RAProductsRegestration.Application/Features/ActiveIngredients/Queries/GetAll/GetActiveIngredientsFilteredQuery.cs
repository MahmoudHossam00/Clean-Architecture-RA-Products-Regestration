using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.Application.Features.ActiveIngredients.Queries.GetAll
{
    public sealed record GetActiveIngredientsFilteredQuery(
    int PageNum = 1,
    string? Filter = null)
    : IQuery<PaginatedList<ActiveIngredientLookupResponse>>;
}
