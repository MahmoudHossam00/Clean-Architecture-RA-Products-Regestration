using RAProductsRegestration.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain
{
    public sealed class Attachment : AuditableEntity, IAggregateRoot
    {
        private Attachment()
        {
        }

        public Attachment(
            string originalFileName,
            string storedFileName,
            string contentType,
            long size,
            string storageKey)
            : base(Guid.NewGuid())
        {
            OriginalFileName = originalFileName;
            StoredFileName = storedFileName;
            ContentType = contentType;
            Size = size;
            StorageKey = storageKey;
        }

        public string OriginalFileName { get; private set; } = null!;

        public string StoredFileName { get; private set; } = null!;

        public string ContentType { get; private set; } = null!;

        public long Size { get; private set; }

        public string StorageKey { get; private set; } = null!;
    }
}
