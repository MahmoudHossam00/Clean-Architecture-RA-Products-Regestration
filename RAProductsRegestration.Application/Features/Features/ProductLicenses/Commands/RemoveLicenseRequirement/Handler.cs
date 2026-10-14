using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.RemoveLicenseRequirement
{
    internal sealed class Handler(
    IProductLicenseRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveLicenseRequirementCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveLicenseRequirementCommand request,
            CancellationToken ct)
        {
            var license =
                await repository
                    .GetByIdWithRequirementsAsync(
                        request.ProductLicenseId,
                        ct);


            if (license is null)
            {
                return ProductLicenseErrors
                    .NotFound(
                        request.ProductLicenseId);
            }


            var result =
                license.RemoveRequirement(
                    request.RequirementId);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Deleted;
        }
    }
}
