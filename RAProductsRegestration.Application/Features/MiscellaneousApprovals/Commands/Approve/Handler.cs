using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Approve
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        ApproveApprovalRequirementCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            ApproveApprovalRequirementCommand request,
            CancellationToken ct)
        {
            var approvalRoot =
                await repository.GetByIdAsync(
                    request.MiscellaneousApprovalId,
                    ct);


            if (approvalRoot is null)
            {
                return MiscellaneousApprovalErrors
                    .NotFound(
                        request.MiscellaneousApprovalId);
            }


            var approval =
                request.Approval
                    .ToDomain();


            var result =
                approvalRoot.ApproveRequirement(
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
