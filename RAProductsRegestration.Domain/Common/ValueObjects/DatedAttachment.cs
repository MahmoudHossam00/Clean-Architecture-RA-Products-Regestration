using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.ValueObjects
{
    public sealed record DatedAttachment
    {
        private DatedAttachment()
        {
        }

        public DatedAttachment(
            DateOnly date,
            AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(attachment);

            Date = date;
            Attachment = attachment;
        }

        public DateOnly Date { get; private init; }

        public AttachmentReference Attachment { get; private init; } = null!;
    }
   
}
