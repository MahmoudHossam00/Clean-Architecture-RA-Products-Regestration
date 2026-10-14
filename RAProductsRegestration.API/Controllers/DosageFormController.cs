using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Features.DosageForms.Commands.Create;
using RAProductsRegestration.Application.Features.DosageForms.Commands.Update;
using RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll;

namespace RAProductsRegestration.API.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DosageFormsController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDosageFormCommand command,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                command,
                ct);

        return result.OnMatch<IActionResult>(
            success => Ok(success),
            errors => BadRequest(new
            {
                errors = errors.Select(error => new
                {
                    error.Code,
                    error.Description
                })
            }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateDosageFormCategoryRequest request,
        CancellationToken ct)
    {
        var command =
            new UpdateDosageFormCommand(
                id,
                request.Name);

        var result =
            await sender.Send(
                command,
                ct);

        return result.OnMatch<IActionResult>(
            success => Ok(success),
            errors => BadRequest(new
            {
                errors = errors.Select(error => new
                {
                    error.Code,
                    error.Description
                })
            }));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetDosageFormsQuery query,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                query,
                ct);

        return result.OnMatch<IActionResult>(
            success => Ok(success),
            errors => BadRequest(new
            {
                errors = errors.Select(error => new
                {
                    error.Code,
                    error.Description
                })
            }));
    }
}

public sealed record UpdateDosageFormCategoryRequest(
    string Name);