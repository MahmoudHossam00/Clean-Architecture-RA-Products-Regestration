using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.ProductTechnicalDocs;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.AddApprovedComposition;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.AddApprovedMoaValidation;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.AddApprovedSpecifications;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.CreateProductTechnicalDocs;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/product-technical-docs")]
public sealed class ProductTechnicalDocsController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Create
    // =========================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateProductTechnicalDocsRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateProductTechnicalDocsCommand(
                request.ProductId),
            ct);
    }


    // =========================================================
    // Get By Product
    // =========================================================

    [HttpGet("product/{productId:guid}")]
    public Task<Result<ProductTechnicalDocsResponse>>
        GetByProductId(
            Guid productId,
            CancellationToken ct)
    {
        return sender.Send(
            new GetProductTechnicalDocsByProductIdQuery(
                productId),
            ct);
    }


    // =========================================================
    // Approved Specifications
    // =========================================================

    [HttpPost("{id:guid}/specifications")]
    public Task<Result<Created>>
        AddApprovedSpecifications(
            Guid id,
            DatedApprovalDocumentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedSpecificationsCommand(
                id,
                request.ApprovalDate,
                request.Attachment),
            ct);
    }


    // =========================================================
    // Approved Composition
    // =========================================================

    [HttpPost("{id:guid}/compositions")]
    public Task<Result<Created>>
        AddApprovedComposition(
            Guid id,
            DatedApprovalDocumentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedCompositionCommand(
                id,
                request.ApprovalDate,
                request.Attachment),
            ct);
    }


    // =========================================================
    // Approved MOA + Validation
    // =========================================================

    [HttpPost("{id:guid}/moa-validations")]
    public Task<Result<Created>>
        AddApprovedMoaValidation(
            Guid id,
            DatedApprovalDocumentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedMoaValidationCommand(
                id,
                request.ApprovalDate,
                request.Attachment),
            ct);
    }
}