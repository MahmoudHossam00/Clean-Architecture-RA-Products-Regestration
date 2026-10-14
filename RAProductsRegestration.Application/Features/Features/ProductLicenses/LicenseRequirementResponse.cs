using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

namespace RAProductsRegestration.Application.Features
    .Features.ProductLicenses;

public sealed record LicenseRequirementResponse(
    Guid Id,
    string Name,
    DateOnly Deadline,
    LookupResponse ResponsibleDepartment,
    AttachmentResponse? RequirementAttachment,
    DatedAttachmentResponse? Approval,
    string? Notes);