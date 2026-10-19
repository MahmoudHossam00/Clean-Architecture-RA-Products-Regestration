using MediatR;
using Microsoft.AspNetCore.Mvc;

using RAProductsRegestration.API.Contracts.Appeals;
using RAProductsRegestration.API.Contracts.ProductLicenses;

using RAProductsRegestration.Application.Common.Models;

using RAProductsRegestration.Application.Features.Features.Appeals;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.Create;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.UpdateDetails;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.SetApproval;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.RemoveApproval;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.Remove;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.AddRequirement;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.UpdateRequirement;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.SetRequirementAttachment;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.RemoveRequirementAttachment;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.SetRequirementApproval;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.RemoveRequirementApproval;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Commands.RemoveRequirement;

using RAProductsRegestration.Application.Features.Features
    .Appeals.Queries.GetByProductId;

using RAProductsRegestration.Domain.Common.Results;


namespace RAProductsRegestration.API.Controllers;


[ApiController]
[Authorize]
[Route("api/appeals")]
public sealed class AppealsController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Get Appeals By Product
    // =========================================================

    [HttpGet("product/{productId:guid}")]
    public Task<Result<AppealResponse[]>>
        GetByProductId(
            Guid productId,
            CancellationToken ct)
    {
        return sender.Send(
            new GetAppealsByProductIdQuery(
                productId),
            ct);
    }


    // =========================================================
    // Create Appeal
    // =========================================================

    [HttpPost]
    public Task<Result<Created>>
        Create(
            CreateAppealRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new CreateAppealCommand(
                request.ProductId,
                request.Initiator,
                request.AppealTypeId,
                request.AppealDate,
                request.Notes),
            ct);
    }


    // =========================================================
    // Update Appeal Details
    // =========================================================

    [HttpPut("{id:guid}/details")]
    public Task<Result<Updated>>
        UpdateDetails(
            Guid id,
            UpdateAppealDetailsRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateAppealDetailsCommand(
                id,
                request.Initiator,
                request.AppealTypeId,
                request.AppealDate,
                request.Notes),
            ct);
    }


    // =========================================================
    // Appeal Approval - Add / Replace
    // =========================================================

    [HttpPut("{id:guid}/approval")]
    public Task<Result<Updated>>
        SetApproval(
            Guid id,
            DatedAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetAppealApprovalCommand(
                id,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Appeal Approval - Remove
    // =========================================================

    [HttpDelete("{id:guid}/approval")]
    public Task<Result<Updated>>
        RemoveApproval(
            Guid id,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveAppealApprovalCommand(
                id),
            ct);
    }


    // =========================================================
    // Delete Appeal
    // =========================================================

    [HttpDelete("{id:guid}")]
    public Task<Result<Deleted>>
        Remove(
            Guid id,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveAppealCommand(
                id),
            ct);
    }


    // =========================================================
    // Requirement - Add
    // =========================================================

    [HttpPost(
        "{id:guid}/requirements")]
    public Task<Result<Created>>
        AddRequirement(
            Guid id,
            AddAppealRequirementRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddAppealRequirementCommand(
                id,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.RequirementAttachment,
                request.Notes),
            ct);
    }


    // =========================================================
    // Requirement - Update
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Updated>>
        UpdateRequirement(
            Guid id,
            Guid requirementId,
            UpdateAppealRequirementRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateAppealRequirementCommand(
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
            new SetAppealRequirementAttachmentCommand(
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
            new RemoveAppealRequirementAttachmentCommand(
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
            new SetAppealRequirementApprovalCommand(
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
            new RemoveAppealRequirementApprovalCommand(
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
            new RemoveAppealRequirementCommand(
                id,
                requirementId),
            ct);
    }
}