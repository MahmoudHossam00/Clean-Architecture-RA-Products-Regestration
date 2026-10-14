using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;
using LookupResponse = RAProductsRegestration.Application.Common.Models.LookupResponse;

namespace RAProductsRegestration.Application.Features
    .MiscellaneousApprovals;

public sealed record MiscellaneousApprovalResponse(
    Guid Id,
    Guid ProductId,

    LookupResponse FileType,

    string Initiator,

    DateOnly SubmissionDate,

    DateOnly? ApprovalDate,

    DateOnly? ApprovalExpiryDate,

    ApprovalRequirementResponse[] ApprovalRequirements);


public sealed record ApprovalRequirementResponse(
    Guid Id,

    string Name,

    DateOnly Deadline,

    LookupResponse ResponsibleDepartment,

    AttachmentResponse? RequirementAttachment,

    DatedAttachmentResponse? Approval,

    string? Notes);