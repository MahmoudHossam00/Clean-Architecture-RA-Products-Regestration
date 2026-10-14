using MediatR;
using Microsoft.AspNetCore.Mvc;

using RAProductsRegestration.API.Contracts.ProductLicenses;
using RAProductsRegestration.API.Contracts.Variations;

using RAProductsRegestration.Application.Common.Models;

using RAProductsRegestration.Application.Features.Features.Variations;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.Create;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.UpdateDetails;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.UpdateDeadline;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.SetApproval;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.RemoveApproval;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.Remove;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.AddRequirement;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.UpdateRequirement;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.SetRequirementAttachment;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.RemoveRequirementAttachment;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.SetRequirementApproval;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.RemoveRequirementApproval;

using RAProductsRegestration.Application.Features.Features
    .Variations.Commands.RemoveRequirement;

using RAProductsRegestration.Application.Features.Features
    .Variations.Queries.GetByProductId;

using RAProductsRegestration.Domain.Common.Results;


namespace RAProductsRegestration.API.Controllers;


[ApiController]
[Route("api/variations")]
public sealed class VariationsController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Create Variation
    // =========================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateVariationRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateVariationCommand(
                request.ProductId,
                request.Initiator,
                request.Cause,
                request.Deadline,
                request.DeadlineNote),
            ct);
    }


    // =========================================================
    // Get Variations By Product
    // =========================================================

    [HttpGet("product/{productId:guid}")]
    public Task<Result<VariationResponse[]>>
        GetByProductId(
            Guid productId,
            CancellationToken ct)
    {
        return sender.Send(
            new GetVariationsByProductIdQuery(
                productId),
            ct);
    }


    // =========================================================
    // Update Variation Details
    // =========================================================

    [HttpPut("{id:guid}/details")]
    public Task<Result<Updated>>
        UpdateDetails(
            Guid id,
            UpdateVariationDetailsRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateVariationDetailsCommand(
                id,
                request.Initiator,
                request.Cause),
            ct);
    }


    // =========================================================
    // Update Variation Deadline
    // =========================================================

    [HttpPut("{id:guid}/deadline")]
    public Task<Result<Updated>>
        UpdateDeadline(
            Guid id,
            UpdateVariationDeadlineRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateVariationDeadlineCommand(
                id,
                request.Deadline,
                request.DeadlineNote),
            ct);
    }


    // =========================================================
    // Variation Approval - Add / Replace
    // =========================================================

    [HttpPut("{id:guid}/approval")]
    public Task<Result<Updated>>
        SetApproval(
            Guid id,
            DatedAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetVariationApprovalCommand(
                id,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Variation Approval - Remove
    // =========================================================

    [HttpDelete("{id:guid}/approval")]
    public Task<Result<Updated>>
        RemoveApproval(
            Guid id,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveVariationApprovalCommand(
                id),
            ct);
    }


    // =========================================================
    // Delete Variation
    // =========================================================

    [HttpDelete("{id:guid}")]
    public Task<Result<Deleted>>
        Remove(
            Guid id,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveVariationCommand(
                id),
            ct);
    }


    // =========================================================
    // Requirements - Add
    // =========================================================

    [HttpPost(
        "{id:guid}/requirements")]
    public Task<Result<Created>>
        AddRequirement(
            Guid id,
            AddVariationRequirementRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddVariationRequirementCommand(
                id,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.RequirementAttachment,
                request.Notes),
            ct);
    }


    // =========================================================
    // Requirements - Update
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Updated>>
        UpdateRequirement(
            Guid id,
            Guid requirementId,
            UpdateVariationRequirementRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateVariationRequirementCommand(
                id,
                requirementId,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.Notes),
            ct);
    }


    // =========================================================
    // Requirement Attachment - Add / Replace
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>>
        SetRequirementAttachment(
            Guid id,
            Guid requirementId,
            AttachmentReferenceInput request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetVariationRequirementAttachmentCommand(
                id,
                requirementId,
                request),
            ct);
    }


    // =========================================================
    // Requirement Attachment - Remove
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>>
        RemoveRequirementAttachment(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveVariationRequirementAttachmentCommand(
                id,
                requirementId),
            ct);
    }


    // =========================================================
    // Requirement Approval - Add / Replace
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>>
        SetRequirementApproval(
            Guid id,
            Guid requirementId,
            DatedAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetVariationRequirementApprovalCommand(
                id,
                requirementId,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Requirement Approval - Remove
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>>
        RemoveRequirementApproval(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveVariationRequirementApprovalCommand(
                id,
                requirementId),
            ct);
    }


    // =========================================================
    // Requirement - Delete
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Deleted>>
        RemoveRequirement(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveVariationRequirementCommand(
                id,
                requirementId),
            ct);
    }
}