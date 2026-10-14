using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.Delete
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        DeleteApprovalRequirementCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            DeleteApprovalRequirementCommand request,
            CancellationToken ct)
        {
            var approval =
                await repository.GetByIdAsync(
                    request.MiscellaneousApprovalId,
                    ct);


            if (approval is null)
            {
                return MiscellaneousApprovalErrors
                    .NotFound(
                        request.MiscellaneousApprovalId);
            }


            var result =
                approval.RemoveApprovalRequirement(
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
