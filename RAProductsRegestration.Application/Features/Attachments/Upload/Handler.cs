using RAProductsRegestration.Application.Common.Interfaces.Files;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .Attachments.Upload;

internal sealed class Handler(
    IAttachmentStorage attachmentStorage)
    : ICommandHandler<
        UploadAttachmentCommand,
        AttachmentUploadResponse>
{
    public async Task<Result<AttachmentUploadResponse>> Handle(
        UploadAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var stored =
            await attachmentStorage.SaveAsync(
                request.File,
                cancellationToken);

        return new AttachmentUploadResponse(
            stored.AttachmentId,
            stored.FileName,
            stored.ContentType,
            stored.Length);
    }
}