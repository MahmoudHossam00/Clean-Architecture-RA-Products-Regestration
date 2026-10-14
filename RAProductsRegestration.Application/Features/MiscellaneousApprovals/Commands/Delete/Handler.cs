using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Delete
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        DeleteMiscellaneousApprovalCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            DeleteMiscellaneousApprovalCommand request,
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


            repository.Remove(
                approval);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Deleted;
        }
    }
}
