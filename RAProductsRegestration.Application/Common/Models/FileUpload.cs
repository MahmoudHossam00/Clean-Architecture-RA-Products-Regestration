using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{

    public sealed record FileUpload(
        Stream Content,
        string FileName,
        string ContentType,
        long Length);

}
