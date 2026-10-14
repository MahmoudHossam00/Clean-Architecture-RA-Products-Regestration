using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.UpdateDetails
{
    internal sealed class Handler(
    IAppealRepository appealRepository,
    IRepository<AppealType> appealTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateAppealDetailsCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateAppealDetailsCommand request,
            CancellationToken ct)
        {
            var appeal =
                await appealRepository
                    .GetByIdAsync(
                        request.AppealId,
                        ct);


            if (appeal is null)
            {
                return AppealErrors.NotFound(
                    request.AppealId);
            }


            var appealTypeExists =
                await appealTypeRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.AppealTypeId,
                    ct);


            if (!appealTypeExists)
            {
                return AppealTypeErrors.NotFound(
                    request.AppealTypeId);
            }


            var result =
                appeal.UpdateDetails(
                    request.Initiator,
                    request.AppealTypeId,
                    request.AppealDate,
                    request.Notes);


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
