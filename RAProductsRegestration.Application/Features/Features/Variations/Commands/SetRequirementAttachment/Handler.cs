using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.SetRequirementAttachment
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetVariationRequirementAttachmentCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetVariationRequirementAttachmentCommand request,
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
                variation.SetRequirementAttachment(
                    request.RequirementId,
                    request.Attachment.ToDomain());


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
