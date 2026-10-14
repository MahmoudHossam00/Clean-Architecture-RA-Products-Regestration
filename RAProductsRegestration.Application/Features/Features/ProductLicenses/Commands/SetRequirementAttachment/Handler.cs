using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetRequirementAttachment
{
    internal sealed class Handler(
    IProductLicenseRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetRequirementAttachmentCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetRequirementAttachmentCommand request,
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
                license.SetRequirementAttachment(
                    request.RequirementId,
                    request.Attachment
                        .ToDomain());


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Updated;
        }
    }
}
