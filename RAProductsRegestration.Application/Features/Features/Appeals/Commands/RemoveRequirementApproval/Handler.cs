using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.RemoveRequirementApproval
{
    internal sealed class Handler(
    IAppealRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveAppealRequirementApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            RemoveAppealRequirementApprovalCommand request,
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


            var result =
                appeal.RemoveRequirementApproval(
                    request.RequirementId);


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
