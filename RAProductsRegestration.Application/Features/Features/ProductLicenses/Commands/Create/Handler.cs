using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.ProductLicenses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.Create
{
    internal sealed class Handler(
      IRepository<ProductLicense> licenseRepository,
      IRepository<LicenseCategory> licenseCategoryRepository,
      IRepository<MarketingType> marketingTypeRepository,
      IUnitOfWork unitOfWork)
      : ICommandHandler<
          CreateProductLicenseCommand,
          Created>
    {
        public async Task<Result<Created>> Handle(
            CreateProductLicenseCommand request,
            CancellationToken ct)
        {
            var alreadyExists =
                await licenseRepository.AnyAsync(
                    x =>
                        x.ProductId ==
                        request.ProductId,
                    ct);

            if (alreadyExists)
            {
                return ProductLicenseErrors
                    .AlreadyExistsForProduct(
                        request.ProductId);
            }


            var licenseCategoryExists =
                await licenseCategoryRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.LicenseCategoryId,
                    ct);

            if (!licenseCategoryExists)
            {
                return LicenseCategoryErrors
                    .NotFound(
                        request.LicenseCategoryId);
            }


            var marketingTypeExists =
                await marketingTypeRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.MarketingTypeId,
                    ct);

            if (!marketingTypeExists)
            {
                return MarketingTypeErrors
                    .NotFound(
                        request.MarketingTypeId);
            }


            var result =
                ProductLicense.Create(
                    request.ProductId,
                    request.LicenseCategoryId,
                    request.IssueDate,
                    request.ValidityDate,
                    request.MarketingTypeId,
                    request.RegistrationNumberYear);

            if (result.IsError)
            {
                return result.TopError!;
            }


            licenseRepository.Add(
                result.Value);

            await unitOfWork
                .SaveChangesAsync(ct);

            return Result.Created;
        }
    }
}
