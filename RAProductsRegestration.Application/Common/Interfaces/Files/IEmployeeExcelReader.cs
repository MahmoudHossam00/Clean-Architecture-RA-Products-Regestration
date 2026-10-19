using RAProductsRegestration.Application.Features.Employees.Import;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Files
{
    public interface IEmployeeExcelReader
    {
        Task<EmployeeExcelRow[]> ReadAsync(
            Stream stream,
            CancellationToken cancellationToken = default);
    }
}
