using RAProductsRegestration.API.Contracts.ProductLicenses;
using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.MiscellaneousApprovals
{

    public sealed record CreateMiscellaneousApprovalRequest(
        Guid ProductId,
        Guid FileTypeId,
        string Initiator,
        DateOnly SubmissionDate);

    public sealed record UpdateMiscellaneousApprovalFileTypeRequest(
    Guid FileTypeId);

    public sealed record UpdateMiscellaneousApprovalDetailsRequest(
    string Initiator,
    DateOnly SubmissionDate);


    public sealed record SetMiscellaneousApprovalRequest(
        DateOnly ApprovalDate,
        DateOnly ApprovalExpiryDate);

    public sealed record AddApprovalRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    AttachmentReferenceInput? RequirementAttachment,
    string? Notes);

    public sealed record UpdateApprovalRequirementRequest(
    string Name,
    DateOnly Deadline,
    Guid ResponsibleDepartmentId,
    string? Notes);


    public sealed record SetApprovalRequirementAttachmentRequest(
        AttachmentReferenceInput Attachment);

    public sealed record ApproveApprovalRequirementRequest(
    DatedAttachmentRequest Approval);
}
