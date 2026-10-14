using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;

namespace RAProductsRegestration.Domain.Appeals.Requirements;

public sealed class AppealRequirement
    : Entity
{
    private AppealRequirement()
    {
    }


    internal AppealRequirement(
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


    internal Result<Updated> UpdateDetails(
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(
            name))
        {
            return AppealRequirementErrors
                .NameRequired;
        }


        if (
            responsibleDepartmentId ==
            Guid.Empty
        )
        {
            return AppealRequirementErrors
                .DepartmentRequired;
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


    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(
            value)
                ? null
                : value.Trim();
    }
}