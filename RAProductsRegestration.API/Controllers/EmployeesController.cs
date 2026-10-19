using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Common.Security;
using RAProductsRegestration.Application.Features
    .Employees.DownloadImportTemplate;
using RAProductsRegestration.Application.Features.Employees.GetAll;
using RAProductsRegestration.Application.Features
    .Employees.Import;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/employees")]
[Authorize]
public sealed class EmployeesController(
    ISender sender)
    : ControllerBase
{

    [HttpGet]
    [Authorize(
    Roles = AppRoles.Admin)]
    public async Task<IActionResult> GetAll(
    CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetAllEmployeesQuery(),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            employees =>
                Ok(employees),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }
    // =========================================================
    // Download Excel Import Template
    // =========================================================

    [HttpGet("import-template")]
    [Authorize(
        Roles = AppRoles.Admin)]
    public async Task<IActionResult> DownloadImportTemplate(
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new DownloadEmployeeImportTemplateQuery(),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            response =>
                File(
                    response.Content,
                    response.ContentType,
                    response.FileName),

            errors =>
                BadRequest(
                    new
                    {
                        errors =
                            errors.Select(
                                error =>
                                    new
                                    {
                                        error.Code,
                                        error.Description
                                    })
                    }));
    }


    // =========================================================
    // Import / Upsert Excel
    // =========================================================

    [HttpPost("import")]
    [Authorize(
        Roles = AppRoles.Admin)]
    [Consumes(
        "multipart/form-data")]
    public async Task<IActionResult> Import(
        [FromForm] ImportEmployeesRequest request,
        CancellationToken cancellationToken)
    {
        if (
            request.File is null ||
            request.File.Length == 0)
        {
            return BadRequest(
                new
                {
                    message =
                        "Excel file is required."
                });
        }


        await using var stream =
            request.File
                .OpenReadStream();


        var result =
            await sender.Send(
                new ImportEmployeesCommand(
                    request.File.FileName,
                    stream),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            response =>
                Ok(response),

            errors =>
                BadRequest(
                    new
                    {
                        errors =
                            errors.Select(
                                error =>
                                    new
                                    {
                                        error.Code,
                                        error.Description
                                    })
                    }));
    }
}


public sealed class ImportEmployeesRequest
{
    public IFormFile File
    {
        get;
        set;
    } = null!;
}