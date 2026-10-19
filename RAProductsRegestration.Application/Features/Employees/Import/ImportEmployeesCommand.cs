using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees.Import
{
    public sealed record ImportEmployeesCommand(
        string FileName,
        Stream Content)
        : ICommand<EmployeeImportResponse>;
}
