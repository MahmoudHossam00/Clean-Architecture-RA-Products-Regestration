using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts
    .ProductLicenses;

public sealed record AddLicenseRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    AttachmentReferenceInput?
        RequirementAttachment,
    string? Notes);



public sealed record UpdateLicenseRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    string? Notes);


