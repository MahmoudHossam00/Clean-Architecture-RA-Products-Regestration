using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.ProductLicenses;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Features.MarketingTypes.Commands.UpdateMarketingType;
using RAProductsRegestration.Application.Features.Features.ProductLicenses;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.AddRequirement;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.Create;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.RemoveLicenseRequirement;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.RemoveRequirementApproval;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.RemoveRequirementAttachment;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetLastApprovedLayout;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetLastApprovedLeaflet;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetRequirementApproval;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.SetRequirementAttachment;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseCategory;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseDates;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseRequirement;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateRegistrationNumber;
using RAProductsRegestration.Application.Features.Features.ProductLicenses.Queries.GetProductLicenseByProductId;

using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController]
[Route("api/product-licenses")]
public sealed class ProductLicensesController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Create
    // =========================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateProductLicenseRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateProductLicenseCommand(
                request.ProductId,
                request.LicenseCategoryId,
                request.IssueDate,
                request.ValidityDate,
                request.MarketingTypeId,
                request.RegistrationNumberYear),
            ct);
    }


    // =========================================================
    // Get By Product
    // =========================================================

    [HttpGet("product/{productId:guid}")]
    public Task<Result<ProductLicenseResponse>>
        GetByProductId(
            Guid productId,
            CancellationToken ct)
    {
        return sender.Send(
            new GetProductLicenseByProductIdQuery(
                productId),
            ct);
    }


    // =========================================================
    // License Category
    // =========================================================

    [HttpPut("{id:guid}/license-category")]
    public Task<Result<Updated>>
        UpdateLicenseCategory(
            Guid id,
            UpdateLicenseCategoryRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateLicenseCategoryCommand(
                id,
                request.LicenseCategoryId),
            ct);
    }


    // =========================================================
    // Marketing Type
    // =========================================================

    [HttpPut("{id:guid}/marketing-type")]
    public Task<Result<Updated>>
        UpdateMarketingType(
            Guid id,
            UpdateMarketingTypeRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateMarketingTypeCommand(
                id,
                request.MarketingTypeId),
            ct);
    }


    // =========================================================
    // Dates
    // =========================================================

    [HttpPut("{id:guid}/dates")]
    public Task<Result<Updated>>
        UpdateDates(
            Guid id,
            UpdateLicenseDatesRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateLicenseDatesCommand(
                id,
                request.IssueDate,
                request.ValidityDate),
            ct);
    }


    // =========================================================
    // Registration Number / Year
    // =========================================================

    [HttpPut("{id:guid}/registration-number")]
    public Task<Result<Updated>>
        UpdateRegistrationNumber(
            Guid id,
            UpdateRegistrationNumberRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateRegistrationNumberCommand(
                id,
                request.RegistrationNumberYear),
            ct);
    }


    // =========================================================
    // Last Approved Leaflet
    // =========================================================

    [HttpPut("{id:guid}/last-approved-leaflet")]
    public Task<Result<Updated>>
        SetLastApprovedLeaflet(
            Guid id,
            DatedAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetLastApprovedLeafletCommand(
                id,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Last Approved Layout
    // =========================================================

    [HttpPut("{id:guid}/last-approved-layout")]
    public Task<Result<Updated>>
        SetLastApprovedLayout(
            Guid id,
            DatedAttachmentRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new SetLastApprovedLayoutCommand(
                id,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Requirements - Add
    // =========================================================

    [HttpPost("{id:guid}/requirements")]
    public Task<Result<Created>> AddRequirement(
        Guid id,
        AddLicenseRequirementRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddLicenseRequirementCommand(
                id,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.RequirementAttachment,
                request.Notes),
            ct);
    }


    // =========================================================
    // Requirements - Update Details
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Updated>> UpdateRequirement(
        Guid id,
        Guid requirementId,
        UpdateLicenseRequirementRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateLicenseRequirementCommand(
                id,
                requirementId,
                request.Name,
                request.Deadline,
                request.ResponsibleDepartmentId,
                request.Notes),
            ct);
    }


    // =========================================================
    // Requirements - Add / Replace Attachment
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>> SetRequirementAttachment(
        Guid id,
        Guid requirementId,
        AttachmentReferenceInput request,
        CancellationToken ct)
    {
        return sender.Send(
            new SetRequirementAttachmentCommand(
                id,
                requirementId,
                request),
            ct);
    }


    // =========================================================
    // Requirements - Remove Attachment
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/attachment")]
    public Task<Result<Updated>> RemoveRequirementAttachment(
        Guid id,
        Guid requirementId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveRequirementAttachmentCommand(
                id,
                requirementId),
            ct);
    }


    // =========================================================
    // Requirements - Approve / Replace Approval
    // =========================================================

    [HttpPut(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>> SetRequirementApproval(
        Guid id,
        Guid requirementId,
        DatedAttachmentRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new SetRequirementApprovalCommand(
                id,
                requirementId,
                request.ToApplication()),
            ct);
    }


    // =========================================================
    // Requirements - Remove Approval
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}/approval")]
    public Task<Result<Updated>> RemoveRequirementApproval(
        Guid id,
        Guid requirementId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveRequirementApprovalCommand(
                id,
                requirementId),
            ct);
    }


    // =========================================================
    // Requirements - Delete Requirement
    // =========================================================

    [HttpDelete(
        "{id:guid}/requirements/{requirementId:guid}")]
    public Task<Result<Deleted>> RemoveRequirement(
        Guid id,
        Guid requirementId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveLicenseRequirementCommand(
                id,
                requirementId),
            ct);
    }

}