using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Features.ActiveIngredients.Queries.GetAll;

namespace RAProductsRegestration.API.Controllers
{
    [ApiController][Authorize]
    [Route("[controller]")]
    [Authorize]
    public sealed class ActiveIngredientsController(
        ISender sender)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetActiveIngredientsFilteredQuery query,
            CancellationToken ct)
        {
            var result = await sender.Send(
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
}