using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.Appeals;


// =========================================================
// Appeal
// =========================================================

public sealed record CreateAppealRequest(
    Guid ProductId,
    string Initiator,
    Guid AppealTypeId,
    DateOnly AppealDate,
    string? Notes);


public sealed record UpdateAppealDetailsRequest(
    string Initiator,
    Guid AppealTypeId,
    DateOnly AppealDate,
    string? Notes);


// =========================================================
// Requirements
// =========================================================

public sealed record AddAppealRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    AttachmentReferenceInput?
        RequirementAttachment,
    string? Notes);


public sealed record UpdateAppealRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    string? Notes);