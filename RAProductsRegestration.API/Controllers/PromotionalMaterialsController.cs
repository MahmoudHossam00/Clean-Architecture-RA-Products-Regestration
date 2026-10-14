using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.PromotionalMaterials;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.Create;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.Delete;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.RemoveApproval;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.ReplaceMaterialFile;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.SetApproval;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.UpdateType;
using RAProductsRegestration.Application.Features
    .PromotionalMaterials.Queries.GetByProductId;

namespace RAProductsRegestration.API.Controllers;

[ApiController]
[Route("api/promotional-materials")]
public sealed class PromotionalMaterialsController(
    ISender sender)
    : ControllerBase
{
    // =====================================================
    // Get By Product
    // =====================================================

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProductId(
        Guid productId,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new GetPromotionalMaterialsByProductIdQuery(
                    productId),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return Ok(
            result.Value);
    }


    // =====================================================
    // Create
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePromotionalMaterialRequest request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new CreatePromotionalMaterialCommand(
                    request.ProductId,
                    request.PromotionalMaterialTypeId,
                    request.MaterialFile),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return StatusCode(
            StatusCodes.Status201Created);
    }


    // =====================================================
    // Update Type
    // =====================================================

    [HttpPut("{id:guid}/type")]
    public async Task<IActionResult> UpdateType(
        Guid id,
        UpdatePromotionalMaterialTypeRequest request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new UpdatePromotionalMaterialTypeCommand(
                    id,
                    request.PromotionalMaterialTypeId),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return NoContent();
    }


    // =====================================================
    // Replace Material File
    // =====================================================

    [HttpPut("{id:guid}/material-file")]
    public async Task<IActionResult> ReplaceMaterialFile(
        Guid id,
        ReplacePromotionalMaterialFileRequest request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new ReplacePromotionalMaterialFileCommand(
                    id,
                    request.MaterialFile),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return NoContent();
    }


    // =====================================================
    // Set / Update Approval
    // =====================================================

    [HttpPut("{id:guid}/approval")]
    public async Task<IActionResult> SetApproval(
        Guid id,
        SetPromotionalMaterialApprovalRequest request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new SetPromotionalMaterialApprovalCommand(
                    id,
                    request.Approval.ToApplication(),
                    request.ApprovalExpiryDate),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return NoContent();
    }


    // =====================================================
    // Remove Approval
    // =====================================================

    [HttpDelete("{id:guid}/approval")]
    public async Task<IActionResult> RemoveApproval(
        Guid id,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new RemovePromotionalMaterialApprovalCommand(
                    id),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return NoContent();
    }


    // =====================================================
    // Delete Promotional Material
    // =====================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new DeletePromotionalMaterialCommand(
                    id),
                ct);


        if (result.IsError)
        {
            return BadRequest(
                result.TopError);
        }


        return NoContent();
    }
}