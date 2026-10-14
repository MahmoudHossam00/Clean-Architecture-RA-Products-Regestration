using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Files
{
    internal sealed class AttachmentStorageOptions
    {
        public string Path { get; set; } =
            AttachmentStorageConstants.DefaultFolder;
    }
}
