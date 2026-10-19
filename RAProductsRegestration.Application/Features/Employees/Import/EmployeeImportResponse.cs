using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees.Import
{
    public sealed record EmployeeImportResponse(
        int TotalRows,
        int Inserted,
        int Updated,
        int Skipped,
        EmployeeImportError[] Errors);


    public sealed record EmployeeImportError(
        int RowNumber,
        string Message);
}
