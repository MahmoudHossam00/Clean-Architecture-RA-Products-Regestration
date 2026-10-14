using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.SetApproval
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetVariationApprovalCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetVariationApprovalCommand request,
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


            var approval =
                request.Approval.ToDomain();


            var result =
                variation.SetApproval(
                    approval.Date,
                    approval.Attachment);


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
