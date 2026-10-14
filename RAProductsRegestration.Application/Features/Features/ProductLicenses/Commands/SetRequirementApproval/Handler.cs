using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetRequirementApproval
{
    internal sealed class Handler(
    IProductLicenseRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetRequirementApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetRequirementApprovalCommand request,
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


            var approval =
                request.Approval
                    .ToDomain();


            var result =
                license.SetRequirementApproval(
                    request.RequirementId,
                    approval.Date,
                    approval.Attachment);


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
