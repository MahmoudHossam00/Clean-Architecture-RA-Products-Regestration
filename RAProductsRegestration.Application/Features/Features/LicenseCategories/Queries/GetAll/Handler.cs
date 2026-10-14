using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.LicenseCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.LicenseCategories.Queries.GetAll
{
    internal sealed class Handler(
    IRepository<LicenseCategory> repository)
    : IQueryHandler<
        GetLicenseCategoriesQuery,
        IReadOnlyCollection<LookupResponse>>
    {
        public async Task<Result<
            IReadOnlyCollection<LookupResponse>>> Handle(
            GetLicenseCategoriesQuery request,
            CancellationToken ct)
        {
            var search =
                request.Search?.Trim();


            var categories =
                string.IsNullOrWhiteSpace(search)

                    ? await repository.WhereAsync(
                        _ => true,
                        ct)

                    : await repository.WhereAsync(
                        x =>
                            x.Name.Contains(search),
                        ct);


            var result =
                categories
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
}
