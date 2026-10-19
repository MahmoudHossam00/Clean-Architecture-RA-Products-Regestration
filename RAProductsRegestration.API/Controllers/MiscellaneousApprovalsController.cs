using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.MiscellaneousApprovals;
using RAProductsRegestration.Application.Features.MiscellaneousApprovals;
using RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Approve;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Create;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Delete;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.RemoveApproval;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.Add;

using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.Delete;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.RemoveApproval;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.RemoveAttachment;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.SetAttachment;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.Requirements.Update;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.SetApproval;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.UpdateDetails;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Commands.UpdateFileType;
using RAProductsRegestration.Application.Features
    .MiscellaneousApprovals.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/miscellaneous-approvals")]
public sealed class MiscellaneousApprovalsController(
    ISender sender)
    : ControllerBase
{
    // =====================================================
    // Get All For Product
    // =====================================================

    [HttpGet("product/{productId:guid}")]
    public Task<Result<MiscellaneousApprovalResponse[]>>
        GetByProductId(
            Guid productId,
            CancellationToken ct)
    {
        return sender.Send(
            new GetMiscellaneousApprovalsByProductIdQuery(
                productId),
            ct);
    }


    // =====================================================
    // Create
    // =====================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateMiscellaneousApprovalRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateMiscellaneousApprovalCommand(
                request.ProductId,
                request.FileTypeId,
                request.Initiator,
                request.SubmissionDate),
            ct);
    }


    // =====================================================
    // Update File Type
    // =====================================================

    [HttpPut("{id:guid}/file-type")]
    public Task<Result<Updated>> UpdateFileType(
        Guid id,
        UpdateMiscellaneousApprovalFileTypeRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateMiscellaneousApprovalFileTypeCommand(
                id,
                request.FileTypeId),
            ct);
    }


    // =====================================================
    // Update Initiator / Submission Date
    // =====================================================

    [HttpPut("{id:guid}/details")]
    public Task<Result<Updated>> UpdateDetails(
        Guid id,
        UpdateMiscellaneousApprovalDetailsRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateMiscellaneousApprovalDetailsCommand(
                id,
                request.Initiator,
                request.SubmissionDate),
            ct);
    }


    // =====================================================
    // Set / Update Main Approval
    // =====================================================

    [HttpPut("{id:guid}/approval")]
    public Task<Result<Updated>> SetApproval(
        Guid id,
        SetMiscellaneousApprovalRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new SetMiscellaneousApprovalCommand(
                id,
                request.ApprovalDate,
                request.ApprovalExpiryDate),
            ct);
    }


    // =====================================================
    // Remove Main Approval
    // =====================================================

    [HttpDelete("{id:guid}/approval")]
    public Task<Result<Updated>> RemoveApproval(
        Guid id,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveMiscellaneousApprovalCommand(
                id),
            ct);
    }


    // =====================================================
    // Delete Miscellaneous Approval
    // =====================================================

    [HttpDelete("{id:guid}")]
    public Task<Result<Deleted>> Delete(
        Guid id,
        CancellationToken ct)
    {
        return sender.Send(
            new DeleteMiscellaneousApprovalCommand(
                id),
            ct);
    }


    // =====================================================
    // Add Requirement
    // =====================================================

    [HttpPost("{id:guid}/requirements")]
    public Task<Result<Created>> AddRequirement(
        Guid id,
        AddApprovalRequirementRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddApprovalRequirementCommand(
                id,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.RequirementAttachment,
                request.Notes),
            ct);
    }


    // =====================================================
    // Update Requirement
    // =====================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Updated>> UpdateRequirement(
        Guid id,
        Guid requirementId,
        UpdateApprovalRequirementRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateApprovalRequirementCommand(
                id,
                requirementId,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.Notes),
            ct);
    }


    // =====================================================
    // Set / Replace Requirement Attachment
    // =====================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>>
        SetRequirementAttachment(
            Guid id,
            Guid requirementId,
            SetApprovalRequirementAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetApprovalRequirementAttachmentCommand(
                id,
                requirementId,
                request.Attachment),
            ct);
    }


    // =====================================================
    // Remove Requirement Attachment
    // =====================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>>
        RemoveRequirementAttachment(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveApprovalRequirementAttachmentCommand(
                id,
                requirementId),
            ct);
    }


    // =====================================================
    // Approve Requirement
    // =====================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>>
        ApproveRequirement(
            Guid id,
            Guid requirementId,
            ApproveApprovalRequirementRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new ApproveApprovalRequirementCommand(
                id,
                requirementId,
                request.Approval.ToApplication()),
            ct);
    }


    // =====================================================
    // Remove Requirement Approval
    // =====================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>>
        RemoveRequirementApproval(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveApprovalRequirementApprovalCommand(
                id,
                requirementId),
            ct);
    }


    // =====================================================
    // Delete Requirement
    // =====================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Deleted>>
        DeleteRequirement(
            Guid id,
            Guid requirementId,
            CancellationToken ct)
    {
        return sender.Send(
            new DeleteApprovalRequirementCommand(
                id,
                requirementId),
            ct);
    }
}