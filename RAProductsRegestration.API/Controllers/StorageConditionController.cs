using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Features.StorageConditions.Commands.Create;
using RAProductsRegestration.Application.Features.StorageConditions.Commands.Update;

//using RAProductsRegestration.Application.Features.StorageConditions.Commands.Update;
using RAProductsRegestration.Application.Features.StorageConditions.Queries.GetAll;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("[controller]")]
public sealed class StorageConditionsController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStorageConditionCommand command,
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
        UpdateStorageConditionCategoryRequest request,
        CancellationToken ct)
    {
        var command =
            new UpdateStorageConditionCommand(
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
        [FromQuery] GetStorageConditionsQuery query,
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

public sealed record UpdateStorageConditionCategoryRequest(
    string Name);