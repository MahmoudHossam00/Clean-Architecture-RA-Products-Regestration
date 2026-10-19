using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees
{
    public sealed record EmployeeImportTemplateResponse(
        byte[] Content,
        string ContentType,
        string FileName);
}
