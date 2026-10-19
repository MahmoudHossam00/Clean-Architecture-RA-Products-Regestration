using RAProductsRegestration.Application.Common.Interfaces.Files;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Employees.DownloadImportTemplate
{

    public sealed record DownloadEmployeeImportTemplateQuery()
        : IQuery<EmployeeImportTemplateResponse>;

    internal sealed class Handler(
    IEmployeeExcelTemplateGenerator templateGenerator)
    : IQueryHandler<
        DownloadEmployeeImportTemplateQuery,
        EmployeeImportTemplateResponse>
    {
        public Task<Result<EmployeeImportTemplateResponse>> Handle(
            DownloadEmployeeImportTemplateQuery request,
            CancellationToken cancellationToken)
        {
            var content =
                templateGenerator.Generate();


            EmployeeImportTemplateResponse response =
                new(
                    Content:
                        content,

                    ContentType:
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

                    FileName:
                        "Employee_Import_Template.xlsx");


            return Task.FromResult<
                Result<EmployeeImportTemplateResponse>>(
                    response);
        }
    }
}
