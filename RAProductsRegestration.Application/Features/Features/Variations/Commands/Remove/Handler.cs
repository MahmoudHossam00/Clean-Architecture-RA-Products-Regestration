using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.Remove
{
    internal sealed class Handler(
        IVariationRepository repository,
        IUnitOfWork unitOfWork)
        : ICommandHandler<
            RemoveVariationCommand,
            Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveVariationCommand request,
            CancellationToken ct)
        {
            var variation =
                await repository
                    .GetByIdWithRequirementsAsync(
                        request.VariationId,
                        ct);


            if (variation is null)
            {
                return VariationErrors.NotFound(
                    request.VariationId);
            }


            repository.Remove(
                variation);


            await unitOfWork.SaveChangesAsync(
                ct);


            return Result.Deleted;
        }
    }
}
