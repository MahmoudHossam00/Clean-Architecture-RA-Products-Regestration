using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features
    .Departments.Queries.GetAll;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController]
[Route("api/departments")]
public sealed class DepartmentsController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Get All / Filter
    // =========================================================

    [HttpGet]
    public Task<Result<
        IReadOnlyCollection<LookupResponse>>>
        GetAll(
            [FromQuery] string? search,
            CancellationToken ct)
    {
        return sender.Send(
            new GetDepartmentsQuery(
                search),
            ct);
    }
}