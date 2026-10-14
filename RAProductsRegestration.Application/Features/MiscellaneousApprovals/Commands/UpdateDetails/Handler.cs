using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.UpdateDetails
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateMiscellaneousApprovalDetailsCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateMiscellaneousApprovalDetailsCommand request,
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
                approval.UpdateDetails(
                    request.Initiator,
                    request.SubmissionDate);


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
