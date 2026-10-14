using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.SetApproval
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetMiscellaneousApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetMiscellaneousApprovalCommand request,
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
                approval.SetApproval(
                    request.ApprovalDate,
                    request.ApprovalExpiryDate);


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
