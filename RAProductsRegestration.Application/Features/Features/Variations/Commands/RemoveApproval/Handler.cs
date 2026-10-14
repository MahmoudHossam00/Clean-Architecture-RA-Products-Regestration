using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.RemoveApproval
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveVariationApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            RemoveVariationApprovalCommand request,
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
                variation.RemoveApproval();


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
