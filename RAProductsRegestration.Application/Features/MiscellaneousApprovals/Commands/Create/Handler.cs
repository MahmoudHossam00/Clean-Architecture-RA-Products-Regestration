using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Create
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IRepository<MiscellaneousApprovalFileType> fileTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateMiscellaneousApprovalCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateMiscellaneousApprovalCommand request,
            CancellationToken ct)
        {
            // =====================================================
            // File Type
            // =====================================================

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


            // =====================================================
            // Domain
            // =====================================================

            var result =
                MiscellaneousApproval.Create(
                    request.ProductId,
                    request.FileTypeId,
                    request.Initiator,
                    request.SubmissionDate);


            if (result.IsError)
            {
                return result.TopError!;
            }


            repository.Add(
                result.Value);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Created;
        }
    }
}
