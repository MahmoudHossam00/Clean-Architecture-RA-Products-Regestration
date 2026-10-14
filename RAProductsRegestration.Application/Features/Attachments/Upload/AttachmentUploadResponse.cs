using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Attachments.Upload
{
    public sealed record AttachmentUploadResponse(
     Guid AttachmentId,
     string FileName,
     string ContentType,
     long Length);
}
