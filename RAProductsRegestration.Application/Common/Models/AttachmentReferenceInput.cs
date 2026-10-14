using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{

    public sealed record AttachmentReferenceInput(
        Guid AttachmentId,
        string FileName,
        string? Notes)
    {
        internal AttachmentReference ToDomain()
        {
            return new AttachmentReference(
                AttachmentId,
                FileName,
                Notes
            );
        }
    }
}
