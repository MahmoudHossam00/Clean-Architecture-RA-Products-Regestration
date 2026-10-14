using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Variations.Requirements;

namespace RAProductsRegestration.Domain.Variations;

public sealed class Variation
    : AuditableEntity, IAggregateRoot
{
    private readonly List<VariationRequirement>
        _requirements = [];


    private Variation()
    {
    }


    private Variation(
        Guid productId,
        string initiator,
        string cause,
        DateOnly deadline,
        string? deadlineNote)
        : base(Guid.NewGuid())
    {
        ProductId =
            productId;

        Initiator =
            initiator;

        Cause =
            cause;

        Deadline =
            deadline;

        DeadlineNote =
            Normalize(deadlineNote);
    }


    // =========================================================
    // Main Information
    // =========================================================

    public Guid ProductId
    {
        get;
        private set;
    }


    public string Initiator
    {
        get;
        private set;
    } = null!;


    public string Cause
    {
        get;
        private set;
    } = null!;


    // =========================================================
    // Variation Approval
    // =========================================================

    public DatedAttachment?
        Approval
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


    public string?
        DeadlineNote
    {
        get;
        private set;
    }


    // =========================================================
    // Requirements
    // =========================================================

    public IReadOnlyCollection<VariationRequirement>
        Requirements =>
            _requirements.AsReadOnly();


    // =========================================================
    // Create
    // =========================================================

    public static Result<Variation> Create(
        Guid productId,
        string initiator,
        string cause,
        DateOnly deadline,
        string? deadlineNote)
    {
        if (productId == Guid.Empty)
        {
            return VariationErrors
                .ProductIdRequired;
        }


        if (string.IsNullOrWhiteSpace(
            initiator))
        {
            return VariationErrors
                .InitiatorRequired;
        }


        if (string.IsNullOrWhiteSpace(
            cause))
        {
            return VariationErrors
                .CauseRequired;
        }


        return new Variation(
            productId,
            initiator.Trim(),
            cause.Trim(),
            deadline,
            deadlineNote);
    }


    // =========================================================
    // Update Main Details
    // =========================================================

    public Result<Updated> UpdateDetails(
        string initiator,
        string cause)
    {
        if (string.IsNullOrWhiteSpace(
            initiator))
        {
            return VariationErrors
                .InitiatorRequired;
        }


        if (string.IsNullOrWhiteSpace(
            cause))
        {
            return VariationErrors
                .CauseRequired;
        }


        Initiator =
            initiator.Trim();

        Cause =
            cause.Trim();


        return Result.Updated;
    }


    // =========================================================
    // Deadline
    // =========================================================

    public Result<Updated> UpdateDeadline(
        DateOnly deadline,
        string? deadlineNote)
    {
        Deadline =
            deadline;

        DeadlineNote =
            Normalize(
                deadlineNote);


        return Result.Updated;
    }


    // =========================================================
    // Approval
    // =========================================================

    public Result<Updated> SetApproval(
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


    public Result<Updated> RemoveApproval()
    {
        Approval =
            null;


        return Result.Updated;
    }


    // =========================================================
    // Requirements - Add
    // =========================================================

    public Result<Created> AddRequirement(
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        AttachmentReference? requirementAttachment,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(
            name))
        {
            return VariationRequirementErrors
                .NameRequired;
        }


        if (
            responsibleDepartmentId ==
            Guid.Empty
        )
        {
            return VariationRequirementErrors
                .DepartmentRequired;
        }


        var requirement =
            new VariationRequirement(
                name.Trim(),
                deadline,
                responsibleDepartmentId,
                requirementAttachment,
                notes);


        _requirements.Add(
            requirement);


        return Result.Created;
    }


    // =========================================================
    // Requirements - Update
    // =========================================================

    public Result<Updated> UpdateRequirement(
        Guid requirementId,
        string name,
        DateOnly deadline,
        Guid responsibleDepartmentId,
        string? notes)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .UpdateDetails(
                name,
                deadline,
                responsibleDepartmentId,
                notes);
    }


    // =========================================================
    // Requirement Attachment
    // =========================================================

    public Result<Updated>
        SetRequirementAttachment(
            Guid requirementId,
            AttachmentReference attachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .SetRequirementAttachment(
                attachment);
    }


    public Result<Updated>
        RemoveRequirementAttachment(
            Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveRequirementAttachment();
    }


    // =========================================================
    // Requirement Approval
    // =========================================================

    public Result<Updated>
        SetRequirementApproval(
            Guid requirementId,
            DateOnly approvalDate,
            AttachmentReference attachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .SetApproval(
                approvalDate,
                attachment);
    }


    public Result<Updated>
        RemoveRequirementApproval(
            Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveApproval();
    }


    // =========================================================
    // Remove Requirement
    // =========================================================

    public Result<Deleted> RemoveRequirement(
        Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return VariationRequirementErrors
                .NotFound(
                    requirementId);
        }


        _requirements.Remove(
            requirement);


        return Result.Deleted;
    }


    // =========================================================
    // Find Requirement
    // =========================================================

    private VariationRequirement?
        FindRequirement(
            Guid requirementId)
    {
        return _requirements
            .FirstOrDefault(
                x =>
                    x.Id ==
                    requirementId);
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