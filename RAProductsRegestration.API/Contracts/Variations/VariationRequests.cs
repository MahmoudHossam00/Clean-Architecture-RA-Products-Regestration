using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.Variations;


// =========================================================
// Variation
// =========================================================

public sealed record CreateVariationRequest(
    Guid ProductId,
    string Initiator,
    string Cause,
    DateOnly Deadline,
    string? DeadlineNote);


public sealed record UpdateVariationDetailsRequest(
    string Initiator,
    string Cause);


public sealed record UpdateVariationDeadlineRequest(
    DateOnly Deadline,
    string? DeadlineNote);


// =========================================================
// Requirement
// =========================================================

public sealed record AddVariationRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    AttachmentReferenceInput? RequirementAttachment,
    string? Notes);


public sealed record UpdateVariationRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    string? Notes);