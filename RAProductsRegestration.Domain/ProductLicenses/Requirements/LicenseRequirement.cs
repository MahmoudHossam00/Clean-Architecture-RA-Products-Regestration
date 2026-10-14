using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;

namespace RAProductsRegestration.Domain.ProductLicenses.Requirements;

public sealed class LicenseRequirement
    : Entity
{
    private LicenseRequirement()
    {
    }


    internal LicenseRequirement(
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
            Normalize(notes);
    }


    // =========================================================
    // Requirement
    // =========================================================

    public string Name
    {
        get;
        private set;
    } = null!;


    // =========================================================
    // Requirement Attachment
    // =========================================================

    public AttachmentReference?
        RequirementAttachment
    {
        get;
        private set;
    }


    // =========================================================
    // Deadline
    // =========================================================

    public DateOnly Deadline
    {
        get;
        private set;
    }


    // =========================================================
    // Responsibility
    // =========================================================

    public Guid ResponsibleDepartmentId
    {
        get;
        private set;
    }


    // =========================================================
    // Approval
    // =========================================================

    public DatedAttachment?
        Approval
    {
        get;
        private set;
    }


    // =========================================================
    // Requirement Notes
    // =========================================================

    public string? Notes
    {
        get;
        private set;
    }


    // =========================================================
    // Update Main Information
    // =========================================================

    internal Result<Updated> UpdateDetails(
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return LicenseRequirementErrors
                .NameRequired;
        }


        if (responsibleDepartmentId == Guid.Empty)
        {
            return LicenseRequirementErrors
                .DepartmentRequired;
        }


        Name =
            name.Trim();


        Deadline =
            deadline;


        ResponsibleDepartmentId =
            responsibleDepartmentId;


        Notes =
            Normalize(notes);


        return Result.Updated;
    }


    // =========================================================
    // Requirement Attachment
    // =========================================================

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


    // =========================================================
    // Approval
    // =========================================================

    internal Result<Updated> SetApproval(
        DateOnly approvalDate,
        AttachmentReference attachment)
    {
        ArgumentNullException.ThrowIfNull(
            attachment);


        Approval =
            new DatedAttachment(
                approvalDate,
                attachment);


        return Result.Updated;
    }


    internal Result<Updated>
        RemoveApproval()
    {
        Approval =
            null;


        return Result.Updated;
    }


    // =========================================================
    // Normalize
    // =========================================================

    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}