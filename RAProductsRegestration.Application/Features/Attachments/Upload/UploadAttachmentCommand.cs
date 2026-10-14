using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Attachments.Upload
{
    public sealed record UploadAttachmentCommand(
            FileUpload File)
            : ICommand<AttachmentUploadResponse>;
}
