using RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Integrations.CTD.Companies.Sync
{

    internal sealed class Handler(
    ICTDDataSource ctdDataSource,
    IRepository<Company> companyRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<SyncCompaniesCommand, Success>
    {
        public async Task<Result<Success>> Handle(
            SyncCompaniesCommand request,
            CancellationToken cancellationToken)
        {
            var ctdCompanies =
                await ctdDataSource.GetCompaniesAsync(
                    cancellationToken);

            if (ctdCompanies.Count == 0)
            {
                return Result.Success;
            }

            // Normalize + remove duplicate CTD company codes once
            var ctdCompaniesByCode = ctdCompanies
                .Where(x => !string.IsNullOrWhiteSpace(x.Code))
                .GroupBy(
                    x => x.Code.Trim(),
                    StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    x => x.Key,
                    x => x.First(),
                    StringComparer.OrdinalIgnoreCase);

            var companyCodes =
                ctdCompaniesByCode.Keys.ToArray();

            // One database query
            var existingCompanies =
                await companyRepository.WhereAsync(
                    x => companyCodes.Contains(x.Code),
                    cancellationToken);

            // Fast in-memory lookup
            var companiesByCode =
                existingCompanies.ToDictionary(
                    x => x.Code,
                    StringComparer.OrdinalIgnoreCase);

            var newCompanies = new List<Company>();

            foreach (var (code, ctdCompany)
                     in ctdCompaniesByCode)
            {
                if (companiesByCode.TryGetValue(
                        code,
                        out var company))
                {
                    company.UpdateFromCTD(
                        ctdCompany.Name);

                    continue;
                }

                var newCompany =
                    new Company(
                        code,
                        ctdCompany.Name);

                newCompanies.Add(newCompany);

                companiesByCode.Add(
                    code,
                    newCompany);
            }

            if (newCompanies.Count > 0)
            {
                companyRepository.AddRange(
                    newCompanies);
            }

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result.Success;
        }
    }
}
