using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.UpdateFileType
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IRepository<MiscellaneousApprovalFileType> fileTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateMiscellaneousApprovalFileTypeCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateMiscellaneousApprovalFileTypeCommand request,
            CancellationToken ct)
        {
            var approval =
                await repository.GetByIdAsync(
                    request.MiscellaneousApprovalId,
                    ct);


            if (approval is null)
            {
                return MiscellaneousApprovalErrors
                    .NotFound(
                        request.MiscellaneousApprovalId);
            }


            var fileTypeExists =
                await fileTypeRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.FileTypeId,
                    ct);


            if (!fileTypeExists)
            {
                return MiscellaneousApprovalFileTypeErrors
                    .NotFound(
                        request.FileTypeId);
            }


            var result =
                approval.UpdateFileType(
                    request.FileTypeId);


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
