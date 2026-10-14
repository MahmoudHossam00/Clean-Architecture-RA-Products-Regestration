using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record DatedApprovalDocumentInput(
        DateOnly ApprovalDate,
        AttachmentReferenceInput Attachment);
}
