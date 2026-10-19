using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Integrations.CTD.ActiveIngredients.Sync;
using RAProductsRegestration.Application.Integrations.CTD.Companies.Sync;
using RAProductsRegestration.Application.Integrations.CTD.Products.sync;
using RAProductsRegestration.Application.Integrations.JDE.Suppliers.Sync;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers
{
    [ApiController][Authorize]
    [Route("[controller]")]
    public class CTDSyncController(
    ISender sender)
    : ControllerBase
    {
        [HttpPost("Companies")]
        public async Task<IActionResult> SyncCompanies(
            SyncCompaniesCommand cmnd,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(
                cmnd,
                cancellationToken);

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
        [HttpPost("Products")]
        public async Task<IActionResult> SyncProducts(
            SyncProductsCommand cmnd,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(
                cmnd,
                cancellationToken);

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
        [HttpPost("ActiveIngredients")]
        public async Task<IActionResult> SyncCompanies(
            SyncActiveIngredientsCommand cmnd,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(
                cmnd,
                cancellationToken);

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
        [HttpPost("Suppliers")]
        public async Task<IActionResult> SyncSuppliers(
            SyncSuppliersCommand cmnd,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(
                cmnd,
                cancellationToken);

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
