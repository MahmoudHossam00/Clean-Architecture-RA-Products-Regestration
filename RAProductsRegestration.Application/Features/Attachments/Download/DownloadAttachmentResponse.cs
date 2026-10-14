using RAProductsRegestration.Application.Common.Interfaces.Messaging;

namespace RAProductsRegestration.Application.Features
    .Attachments.Download;

public sealed record DownloadAttachmentResponse(
    Stream Content,
    string FileName);