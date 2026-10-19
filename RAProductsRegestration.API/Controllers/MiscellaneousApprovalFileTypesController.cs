using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovalFileTypes.Commands.Create;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovalFileTypes.Queries.GetAll;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/miscellaneous-approval-file-types")]
public sealed class MiscellaneousApprovalFileTypesController(
    ISender sender)
    : ControllerBase
{
    // =====================================================
    // Create
    // =====================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateMiscellaneousApprovalFileTypeRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateMiscellaneousApprovalFileTypeCommand(
                request.Name),
            ct);
    }


    // =====================================================
    // Get All / Search
    // =====================================================

    [HttpGet]
    public Task<Result<LookupResponse[]>> GetAll(
        [FromQuery] string? search,
        CancellationToken ct)
    {
        return sender.Send(
            new GetMiscellaneousApprovalFileTypesQuery(
                search),
            ct);
    }
}