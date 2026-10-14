using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.MarketingTypes.Commands.UpdateMarketingType
{
    internal sealed class Handler(
    IRepository<ProductLicense> licenseRepository,
    IRepository<MarketingType> marketingTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateMarketingTypeCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateMarketingTypeCommand request,
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


            var marketingTypeExists =
                await marketingTypeRepository
                    .AnyAsync(
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
                license.UpdateMarketingType(
                    request.MarketingTypeId);

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
