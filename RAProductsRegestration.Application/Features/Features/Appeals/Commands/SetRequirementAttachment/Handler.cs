using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.SetRequirementAttachment
{
    internal sealed class Handler(
    IAppealRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        SetAppealRequirementAttachmentCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetAppealRequirementAttachmentCommand request,
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
                appeal.SetRequirementAttachment(
                    request.RequirementId,
                    request.Attachment.ToDomain());


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
