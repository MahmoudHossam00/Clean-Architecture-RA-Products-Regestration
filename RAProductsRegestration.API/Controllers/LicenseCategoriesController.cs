using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.LicenseCategories;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Features.LicenseCategories.Queries.GetAll;
using RAProductsRegestration.Application.Features.LicenseCategories.Commands.Create;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/license-categories")]
public sealed class LicenseCategoriesController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    public Task<Result<Created>> Create(
        CreateLicenseCategoryRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateLicenseCategoryCommand(
                request.Name),
            ct);
    }


    [HttpGet]
    public Task<Result<
        IReadOnlyCollection<LookupResponse>>> GetAll(
        [FromQuery] string? search,
        CancellationToken ct)
    {
        return sender.Send(
            new GetLicenseCategoriesQuery(
                search),
            ct);
    }
}