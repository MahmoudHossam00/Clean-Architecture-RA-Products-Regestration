using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.PromotionalMaterialTypes;
using RAProductsRegestration.Application.Features
    .PromotionalMaterialTypes.Commands.Create;
using RAProductsRegestration.Application.Features
    .PromotionalMaterialTypes.Queries.GetAll;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/promotional-material-types")]
public sealed class PromotionalMaterialTypesController(
    ISender sender)
    : ControllerBase
{
    // =====================================================
    // Create
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePromotionalMaterialTypeRequest request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new CreatePromotionalMaterialTypeCommand(
                    request.Name),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return StatusCode(
            StatusCodes.Status201Created);
    }


    // =====================================================
    // Get All
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new GetPromotionalMaterialTypesQuery(
                    search),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return Ok(
            result.Value);
    }
}