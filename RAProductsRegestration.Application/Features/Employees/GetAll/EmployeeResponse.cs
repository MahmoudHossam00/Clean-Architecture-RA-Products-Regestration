using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees.GetAll
{
    public sealed record EmployeeResponse(
        Guid Id,
        long ZenId,
        int EmployeeNumber,
        string NameEn,
        string? NameAr,
        string? Email,
        Guid? DepartmentId);
    public sealed record GetAllEmployeesQuery()
    : IQuery<EmployeeResponse[]>;

    internal sealed class Handler(
    IRepository<Employee> repository)
    : IQueryHandler<
        GetAllEmployeesQuery,
        EmployeeResponse[]>
    {
        public async Task<Result<EmployeeResponse[]>> Handle(
            GetAllEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var employees =
                await repository.WhereAsync(
                    _ => true,
                    cancellationToken);


            return employees
                .OrderBy(
                    x =>
                        x.EmployeeNumber)
                .Select(
                    x =>
                        new EmployeeResponse(
                            x.Id,
                            x.ZenId,
                            x.EmployeeNumber,
                            x.NameEn,
                            x.NameAr,
                            x.Email,
                            x.DepartmentId))
                .ToArray();
        }
    }
}
