using MediatR;
using Microsoft.AspNetCore.Mvc;

using RAProductsRegestration
    .Application
    .Features
    .Suppliers
    .Queries
    .GetAll;

namespace RAProductsRegestration
    .API.Controllers;

[ApiController][Authorize]
[Route("[controller]")]
public sealed class SuppliersController(
    ISender sender)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery]
        GetSuppliersFilteredQuery query,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                query,
                ct);

        return result.OnMatch<IActionResult>(
            success =>
                Ok(success),

            errors =>
                BadRequest(new
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