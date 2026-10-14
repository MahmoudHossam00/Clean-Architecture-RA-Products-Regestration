using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;

namespace RAProductsRegestration.Domain
    .MiscellaneousApprovals.Requirements;

public sealed class ApprovalRequirement
    : Entity
{
    private ApprovalRequirement()
    {
    }


    internal ApprovalRequirement(
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        AttachmentReference? requirementAttachment,
        string? notes)
        : base(Guid.NewGuid())
    {
        Name =
            name;

        Deadline =
            deadline;

        ResponsibleDepartmentId =
            responsibleDepartmentId;

        RequirementAttachment =
            requirementAttachment;

        Notes =
            Normalize(
                notes);
    }


    // =====================================================
    // Properties
    // =====================================================

    public string Name
    {
        get;
        private set;
    } = null!;


    public DateOnly Deadline
    {
        get;
        private set;
    }


    public Guid ResponsibleDepartmentId
    {
        get;
        private set;
    }


    public AttachmentReference?
        RequirementAttachment
    {
        get;
        private set;
    }


    public DatedAttachment?
        Approval
    {
        get;
        private set;
    }


    public string? Notes
    {
        get;
        private set;
    }


    // =====================================================
    // Update
    // =====================================================

    internal Result<Updated> Update(
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ApprovalRequirementErrors
                .NameRequired;
        }


        if (
            responsibleDepartmentId ==
            Guid.Empty
        )
        {
            return ApprovalRequirementErrors
                .ResponsibleDepartmentRequired;
        }


        Name =
            name.Trim();

        Deadline =
            deadline;

        ResponsibleDepartmentId =
            responsibleDepartmentId;

        Notes =
            Normalize(
                notes);


        return Result.Updated;
    }


    // =====================================================
    // Requirement Attachment
    // =====================================================

    internal Result<Updated>
        SetRequirementAttachment(
            AttachmentReference attachment)
    {
        ArgumentNullException.ThrowIfNull(
            attachment);


        RequirementAttachment =
            attachment;


        return Result.Updated;
    }


    internal Result<Updated>
        RemoveRequirementAttachment()
    {
        RequirementAttachment =
            null;


        return Result.Updated;
    }


    // =====================================================
    // Approval
    // =====================================================

    internal Result<Updated> Approve(
        DateOnly approvalDate,
        AttachmentReference approvalAttachment)
    {
        ArgumentNullException.ThrowIfNull(
            approvalAttachment);


        Approval =
            new DatedAttachment(
                approvalDate,
                approvalAttachment);


        return Result.Updated;
    }


    internal Result<Updated>
        RemoveApproval()
    {
        Approval =
            null;


        return Result.Updated;
    }


    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}