using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll;
using RAProductsRegestration.Application.Features.Products.Queries.GetAll;

namespace RAProductsRegestration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(
    ISender sender)
    : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] GetProductsFilteredQuery query,
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
}
