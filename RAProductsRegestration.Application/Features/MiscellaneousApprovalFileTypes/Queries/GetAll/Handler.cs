using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovalFileTypes.Queries.GetAll
{
    internal sealed class Handler(
    IRepository<MiscellaneousApprovalFileType> repository)
    : IQueryHandler<
        GetMiscellaneousApprovalFileTypesQuery,
        LookupResponse[]>
    {
        public async Task<Result<LookupResponse[]>>
            Handle(
                GetMiscellaneousApprovalFileTypesQuery request,
                CancellationToken ct)
        {
            var search =
                request.Search?.Trim();


            IReadOnlyList<MiscellaneousApprovalFileType>
                fileTypes;


            if (string.IsNullOrWhiteSpace(search))
            {
                fileTypes =
                    await repository.WhereAsync(
                        x => true,
                        ct);
            }
            else
            {
                fileTypes =
                    await repository.WhereAsync(
                        x =>
                            x.Name.Contains(
                                search),
                        ct);
            }


            var response =
                fileTypes
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
