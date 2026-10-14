using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.Create
{
    internal sealed class Handler(
    IAppealRepository appealRepository,
    IRepository<AppealType> appealTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateAppealCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateAppealCommand request,
            CancellationToken ct)
        {
            // =====================================================
            // Appeal Type
            // =====================================================

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


            // =====================================================
            // Domain
            // =====================================================

            var result =
                Appeal.Create(
                    request.ProductId,
                    request.Initiator,
                    request.AppealTypeId,
                    request.AppealDate,
                    request.Notes);


            if (result.IsError)
            {
                return result.TopError!;
            }


            appealRepository.Add(
                result.Value);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Created;
        }
    }
}
