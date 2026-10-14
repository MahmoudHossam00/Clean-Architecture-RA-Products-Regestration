using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.StorageConditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll
{
    internal sealed class Handler(
    IRepository<StorageCondition> repository)
    : IQueryHandler<
        GetStorageConditionsQuery,
        IReadOnlyCollection<StorageConditionLookupResponse>>
    {
        public async Task<Result<
            IReadOnlyCollection<StorageConditionLookupResponse>>> Handle(
            GetStorageConditionsQuery request,
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
                    new StorageConditionLookupResponse(
                        x.Id,
                        x.Name))
                .ToList();
        }
    }
}
