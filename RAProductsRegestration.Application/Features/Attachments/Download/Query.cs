using RAProductsRegestration.Application.Common.Interfaces.Messaging;

namespace RAProductsRegestration.Application.Features
    .Attachments.Download;

public sealed record DownloadAttachmentQuery(
    Guid AttachmentId,
    string FileName)
    : IQuery<DownloadAttachmentResponse>;
