using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseCategory
{
    internal sealed class Handler(
    IRepository<ProductLicense> licenseRepository,
    IRepository<LicenseCategory> categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateLicenseCategoryCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateLicenseCategoryCommand request,
            CancellationToken ct)
        {
            var license =
                await licenseRepository
                    .GetByIdAsync(
                        request.ProductLicenseId,
                        ct);

            if (license is null)
            {
                return ProductLicenseErrors
                    .NotFound(
                        request.ProductLicenseId);
            }


            var categoryExists =
                await categoryRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.LicenseCategoryId,
                    ct);

            if (!categoryExists)
            {
                return LicenseCategoryErrors
                    .NotFound(
                        request.LicenseCategoryId);
            }


            var result =
                license.UpdateLicenseCategory(
                    request.LicenseCategoryId);

            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(ct);

            return Result.Updated;
        }
    }
}
