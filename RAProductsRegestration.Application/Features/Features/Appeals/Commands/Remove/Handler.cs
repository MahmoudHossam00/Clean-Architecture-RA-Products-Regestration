using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.Remove
{
    internal sealed class Handler(
    IAppealRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveAppealCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveAppealCommand request,
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


            repository.Remove(
                appeal);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Deleted;
        }
    }
}
