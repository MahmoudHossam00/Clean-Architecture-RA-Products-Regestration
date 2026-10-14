using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.UpdateDeadline
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateVariationDeadlineCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateVariationDeadlineCommand request,
            CancellationToken ct)
        {
            var variation =
                await repository.GetByIdAsync(
                    request.VariationId,
                    ct);


            if (variation is null)
            {
                return VariationErrors.NotFound(
                    request.VariationId);
            }


            var result =
                variation.UpdateDeadline(
                    request.Deadline,
                    request.DeadlineNote);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork.SaveChangesAsync(
                ct);


            return Result.Updated;
        }
    }
}
