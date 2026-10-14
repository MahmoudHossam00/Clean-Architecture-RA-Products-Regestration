using RAProductsRegestration.Application.Common.Interfaces.Files;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features
    .Attachments.Download;

internal sealed class Handler(
    IAttachmentStorage attachmentStorage)
    : IQueryHandler<
        DownloadAttachmentQuery,
        DownloadAttachmentResponse>
{
    public async Task<Result<DownloadAttachmentResponse>> Handle(
        DownloadAttachmentQuery request,
        CancellationToken cancellationToken)
    {
        var stream =
            await attachmentStorage.OpenReadAsync(
                request.AttachmentId,
                cancellationToken);

        if (stream is null)
        {
            return Error.NotFound(
                "Attachment.NotFound",
                $"Attachment '{request.AttachmentId}' was not found.");
        }

        var safeFileName =
            Path.GetFileName(
                request.FileName);

        return new DownloadAttachmentResponse(
            stream,
            safeFileName);
    }
}