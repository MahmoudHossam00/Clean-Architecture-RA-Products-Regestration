using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees.Import
{
    public sealed record EmployeeExcelRow(
        int RowNumber,
        string? ZenId,
        string? EmployeeNumber,
        string? NameEn,
        string? NameAr,
        string? Email,
        string? DepartmentId);
}
