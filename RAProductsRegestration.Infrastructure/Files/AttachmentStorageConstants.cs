using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Files
{
    internal static class AttachmentStorageConstants
    {
        public const string ConfigurationSection =
            "AttachmentStorage";

        public const string DefaultFolder =
            "App_Data/Attachments";

        public const int BufferSize =
            81920;
    }
}
