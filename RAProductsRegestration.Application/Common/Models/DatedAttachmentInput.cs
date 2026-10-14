using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record DatedAttachmentInput(
    DateOnly Date,
    Guid AttachmentId,
    string FileName,
    string? Notes)
    {
        internal DatedAttachment ToDomain()
        {
            return new DatedAttachment(
                Date,
                new AttachmentReference(
                    AttachmentId,
                    FileName,
                    Notes));
        }
    }
}
