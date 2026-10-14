using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ProductsTechnicalDocs.Common
{
    public sealed record DatedApprovalDocument
    {
        private DatedApprovalDocument()
        {
        }

        public DatedApprovalDocument(
            DateOnly approvalDate,
            AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(attachment);

            ApprovalDate = approvalDate;
            Attachment = attachment;
        }

        public DateOnly ApprovalDate
        {
            get;
            private init;
        }

        public AttachmentReference Attachment
        {
            get;
            private init;
        } = null!;
    }
}
