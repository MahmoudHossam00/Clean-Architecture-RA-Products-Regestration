using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.MarketingTypes;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Features.MarketingTypes.Commands.Create;
using RAProductsRegestration.Application.Features.Features.MarketingTypes.Queries.GetAll;
using RAProductsRegestration.Application.Features.MarketingTypes.Queries.GetAll;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController]
[Route("api/marketing-types")]
public sealed class MarketingTypesController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    public Task<Result<Created>> Create(
        CreateMarketingTypeRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateMarketingTypeCommand(
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
            new GetMarketingTypesQuery(
                search),
            ct);
    }
}