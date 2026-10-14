using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.AppealTypes.Commands.Queries.GetAll
{
    internal sealed class Handler(
        IRepository<AppealType> repository)
        : IQueryHandler<
            GetAppealTypesQuery,
            LookupResponse[]>
    {
        public async Task<Result<LookupResponse[]>> Handle(
            GetAppealTypesQuery request,
            CancellationToken ct)
        {
            var search =
                request.Search?.Trim();


            var appealTypes =
                string.IsNullOrWhiteSpace(
                    search)

                    ? await repository.WhereAsync(
                        _ => true,
                        ct)

                    : await repository.WhereAsync(
                        x =>
                            x.Name.Contains(
                                search),
                        ct);


            return appealTypes
                .OrderBy(
                    x => x.Name)
                .Select(
                    x =>
                        new LookupResponse(
                            x.Id,
                            x.Name))
                .ToArray();
        }
    }
}
