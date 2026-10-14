using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.SetRequirementApproval
{
    internal sealed class Handler(
        IAppealRepository repository,
        IUnitOfWork unitOfWork)
        : ICommandHandler<
            SetAppealRequirementApprovalCommand,
            Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetAppealRequirementApprovalCommand request,
            CancellationToken ct)
        {
            var appeal =
                await repository
                    .GetByIdWithRequirementsAsync(
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
                appeal.SetRequirementApproval(
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
