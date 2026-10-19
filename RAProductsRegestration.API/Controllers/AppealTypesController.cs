using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.AppealTypes;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Features
    .AppealTypes.Commands.Create;
using RAProductsRegestration.Application.Features.Features.AppealTypes.Commands.Queries.GetAll;

using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;


[ApiController][Authorize]
[Route("api/appeal-types")]
public sealed class AppealTypesController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Create
    // =========================================================

    [HttpPost]
    public Task<Result<Created>>
        Create(
            CreateAppealTypeRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new CreateAppealTypeCommand(
                request.Name),
            ct);
    }


    // =========================================================
    // Get All / Search
    // =========================================================

    [HttpGet]
    public Task<Result<LookupResponse[]>>
        GetAll(
            [FromQuery] string? search,
            CancellationToken ct)
    {
        return sender.Send(
            new GetAppealTypesQuery(
                search),
            ct);
    }
}