using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.SetApproval
{
    internal sealed class Handler(
    IAppealRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetAppealApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetAppealApprovalCommand request,
            CancellationToken ct)
        {
            var appeal =
                await repository.GetByIdAsync(
                    request.AppealId,
                    ct);


            if (appeal is null)
            {
                return AppealErrors.NotFound(
                    request.AppealId);
            }


            var approval =
                request.Approval.ToDomain();


            var result =
                appeal.SetApproval(
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
