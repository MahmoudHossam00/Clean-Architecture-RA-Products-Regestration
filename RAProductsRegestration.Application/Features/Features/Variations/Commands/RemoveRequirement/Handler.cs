using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.RemoveRequirement
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        RemoveVariationRequirementCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            RemoveVariationRequirementCommand request,
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


            var result =
                variation.RemoveRequirement(
                    request.RequirementId);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork.SaveChangesAsync(
                ct);


            return Result.Deleted;
        }
    }
}
