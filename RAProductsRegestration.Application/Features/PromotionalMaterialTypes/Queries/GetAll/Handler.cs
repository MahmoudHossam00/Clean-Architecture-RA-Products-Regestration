using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;

namespace RAProductsRegestration.Application.Features.PromotionalMaterialTypes.Queries.GetAll
{
    internal sealed class Handler(
        IRepository<PromotionalMaterialType> repository)
        : IQueryHandler<
            GetPromotionalMaterialTypesQuery,
            LookupResponse[]>
    {
        public async Task<Result<LookupResponse[]>>
            Handle(
                GetPromotionalMaterialTypesQuery request,
                CancellationToken ct)
        {
            var search =
                request.Search?.Trim();


            IReadOnlyList<PromotionalMaterialType> types;


            if (string.IsNullOrWhiteSpace(search))
            {
                types =
                    await repository.WhereAsync(
                        x => true,
                        ct);
            }
            else
            {
                types =
                    await repository.WhereAsync(
                        x =>
                            x.Name.Contains(search),
                        ct);
            }


            var response =
                types
                    .OrderBy(
                        x => x.Name)
                    .Select(
                        x =>
                            new LookupResponse(
                                x.Id,
                                x.Name))
                    .ToArray();


            return response;
        }
    }
}
