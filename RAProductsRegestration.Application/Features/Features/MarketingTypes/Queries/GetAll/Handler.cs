
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Features.MarketingTypes.Queries.GetAll;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MarketingTypes;

namespace RAProductsRegestration.Application.Features
    .MarketingTypes.Queries.GetAll;

internal sealed class Handler(
    IRepository<MarketingType> repository)
    : IQueryHandler<
        GetMarketingTypesQuery,
        IReadOnlyCollection<LookupResponse>>
{
    public async Task<Result<
        IReadOnlyCollection<LookupResponse>>> Handle(
        GetMarketingTypesQuery request,
        CancellationToken ct)
    {
        var search =
            request.Search?.Trim();


        var marketingTypes =
            string.IsNullOrWhiteSpace(search)

                ? await repository.WhereAsync(
                    _ => true,
                    ct)

                : await repository.WhereAsync(
                    x =>
                        x.Name.Contains(search),
                    ct);


        var result =
            marketingTypes
                .OrderBy(
                    x => x.Name)
                .Select(
                    x => new LookupResponse(
                        x.Id,
                        x.Name))
                .ToArray();


        return result;
    }
}