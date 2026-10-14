using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Files
{
    public interface IAttachmentStorage
    {
        Task<StoredAttachment> SaveAsync(
            FileUpload file,
            CancellationToken cancellationToken = default);

        Task<Stream?> OpenReadAsync(
            Guid attachmentId,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(
            Guid attachmentId,
            CancellationToken cancellationToken = default);
    }
    

}
