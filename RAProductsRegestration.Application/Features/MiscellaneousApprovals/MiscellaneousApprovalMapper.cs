using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;
using RAProductsRegestration.Domain.MiscellaneousApprovals.Requirements;
using LookupResponse = RAProductsRegestration.Application.Common.Models.LookupResponse;

namespace RAProductsRegestration.Application.Features
    .MiscellaneousApprovals;

internal static class MiscellaneousApprovalMapper
{
    public static MiscellaneousApprovalResponse[] Map(
        IReadOnlyCollection<MiscellaneousApproval> approvals,
        IReadOnlyCollection<MiscellaneousApprovalFileType> fileTypes,
        IReadOnlyCollection<Department> departments)
    {
        var fileTypeMap =
            fileTypes.ToDictionary(
                x => x.Id);


        var departmentMap =
            departments.ToDictionary(
                x => x.Id);


        return approvals
            .OrderByDescending(
                x => x.SubmissionDate)
            .Select(
                approval =>
                    Map(
                        approval,
                        fileTypeMap,
                        departmentMap))
            .ToArray();
    }


    // =====================================================
    // Miscellaneous Approval
    // =====================================================

    private static MiscellaneousApprovalResponse Map(
        MiscellaneousApproval approval,
        IReadOnlyDictionary<
            Guid,
            MiscellaneousApprovalFileType> fileTypes,
        IReadOnlyDictionary<
            Guid,
            Department> departments)
    {
        if (!fileTypes.TryGetValue(
                approval.FileTypeId,
                out var fileType))
        {
            throw new InvalidOperationException(
                $"Miscellaneous approval file type '{approval.FileTypeId}' was not found.");
        }


        var requirements =
            approval.ApprovalRequirements
                .OrderBy(
                    x => x.Deadline)
                .Select(
                    requirement =>
                        MapRequirement(
                            requirement,
                            departments))
                .ToArray();


        return new MiscellaneousApprovalResponse(
            approval.Id,

            approval.ProductId,

            new LookupResponse(
                fileType.Id,
                fileType.Name),

            approval.Initiator,

            approval.SubmissionDate,

            approval.ApprovalDate,

            approval.ApprovalExpiryDate,

            requirements);
    }


    // =====================================================
    // Requirement
    // =====================================================

    private static ApprovalRequirementResponse
        MapRequirement(
            ApprovalRequirement requirement,
            IReadOnlyDictionary<
                Guid,
                Department> departments)
    {
        if (!departments.TryGetValue(
                requirement.ResponsibleDepartmentId,
                out var department))
        {
            throw new InvalidOperationException(
                $"Department '{requirement.ResponsibleDepartmentId}' was not found.");
        }


        return new ApprovalRequirementResponse(
            requirement.Id,

            requirement.Name,

            requirement.Deadline,

            new LookupResponse(
                department.Id,
                department.Name),

            MapAttachment(
                requirement.RequirementAttachment),

            MapDatedAttachment(
                requirement.Approval),

            requirement.Notes);
    }


    // =====================================================
    // Attachment
    // =====================================================

    private static AttachmentResponse?
        MapAttachment(
            AttachmentReference? attachment)
    {
        if (attachment is null)
        {
            return null;
        }


        return new AttachmentResponse(
            attachment.AttachmentId,
            attachment.FileName,
            attachment.Notes);
    }


    // =====================================================
    // Dated Attachment
    // =====================================================

    private static DatedAttachmentResponse?
        MapDatedAttachment(
            DatedAttachment? attachment)
    {
        if (attachment is null)
        {
            return null;
        }


        return new DatedAttachmentResponse(
            attachment.Date,

            new AttachmentResponse(
                attachment.Attachment.AttachmentId,
                attachment.Attachment.FileName,
                attachment.Attachment.Notes));
    }
}