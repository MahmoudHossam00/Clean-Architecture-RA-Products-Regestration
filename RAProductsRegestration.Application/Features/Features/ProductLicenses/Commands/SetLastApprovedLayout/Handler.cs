using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetLastApprovedLayout
{
    internal sealed class Handler(
    IRepository<ProductLicense> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetLastApprovedLayoutCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetLastApprovedLayoutCommand request,
            CancellationToken ct)
        {
            var license =
                await repository.GetByIdAsync(
                    request.ProductLicenseId,
                    ct);

            if (license is null)
            {
                return ProductLicenseErrors
                    .NotFound(
                        request.ProductLicenseId);
            }


            var approval =
                request.Approval.ToDomain();


            var result =
                license.SetLastApprovedLayout(
                    approval.Date,
                    approval.Attachment);

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
