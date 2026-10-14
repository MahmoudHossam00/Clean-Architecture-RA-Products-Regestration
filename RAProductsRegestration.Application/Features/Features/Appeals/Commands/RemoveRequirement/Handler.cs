using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.RemoveRequirement
{
    internal sealed class Handler(
    IAppealRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveAppealRequirementCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveAppealRequirementCommand request,
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
                appeal.RemoveRequirement(
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
