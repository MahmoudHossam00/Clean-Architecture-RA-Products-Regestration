using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record ShelfLifeInput(
        int NumberOfMonths,
        DateOnly ApprovalDate,
        IReadOnlyCollection<AttachmentReferenceInput> Attachments)
    {
        internal IReadOnlyCollection<AttachmentReference>
            ToDomainAttachments()
        {
            return Attachments
                .Select(x => x.ToDomain())
                .ToArray();
        }
    }
}
