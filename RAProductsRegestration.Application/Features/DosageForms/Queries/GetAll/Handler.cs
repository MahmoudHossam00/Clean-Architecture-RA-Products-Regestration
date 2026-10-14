using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll
{
    internal sealed class Handler(
    IRepository<DosageForm> repository)
    : IQueryHandler<
        GetDosageFormsQuery,
        IReadOnlyCollection<DosageFormLookupResponse>>
    {
        public async Task<Result<
            IReadOnlyCollection<DosageFormLookupResponse>>> Handle(
            GetDosageFormsQuery request,
            CancellationToken cancellationToken)
        {
            var items =
                request.ActiveOnly
                    ? await repository.WhereAsync(
                        x => x.IsActive,
                        cancellationToken)
                    : await repository.WhereAsync(
                        x => true,
                        cancellationToken);

            return items
                .OrderBy(x => x.Name)
                .Select(x =>
                    new DosageFormLookupResponse(
                        x.Id,
                        x.Name))
                .ToList();
        }
    }
}
