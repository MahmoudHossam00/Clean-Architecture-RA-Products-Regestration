using RAProductsRegestration.Domain.Appeals.Requirements;
using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using System.Xml.Linq;

namespace RAProductsRegestration.Domain.Appeals;

public sealed class Appeal
    : AuditableEntity, IAggregateRoot
{
    private readonly List<AppealRequirement>
        _requirements = [];


    private Appeal()
    {
    }


    private Appeal(
        Guid productId,
        string initiator,
        Guid appealTypeId,
        DateOnly appealDate,
        string? notes)
        : base(Guid.NewGuid())
    {
        ProductId =
            productId;

        Initiator =
            initiator;

        AppealTypeId =
            appealTypeId;

        AppealDate =
            appealDate;

        Notes =
            Normalize(
                notes);
    }


    // =========================================================
    // Main Data
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


    public Guid AppealTypeId
    {
        get;
        private set;
    }


    public DateOnly AppealDate
    {
        get;
        private set;
    }


    public string? Notes
    {
        get;
        private set;
    }


    // =========================================================
    // Approval
    // =========================================================

    public DatedAttachment? Approval
    {
        get;
        private set;
    }


    // =========================================================
    // Requirements
    // =========================================================

    public IReadOnlyCollection<AppealRequirement>
        Requirements =>
            _requirements.AsReadOnly();


    // =========================================================
    // Create
    // =========================================================

    public static Result<Appeal> Create(
        Guid productId,
        string initiator,
        Guid appealTypeId,
        DateOnly appealDate,
        string? notes)
    {
        if (productId == Guid.Empty)
        {
            return AppealErrors
                .ProductIdRequired;
        }


        if (string.IsNullOrWhiteSpace(
            initiator))
        {
            return AppealErrors
                .InitiatorRequired;
        }


        if (appealTypeId == Guid.Empty)
        {
            return AppealErrors
                .AppealTypeRequired;
        }


        return new Appeal(
            productId,
            initiator.Trim(),
            appealTypeId,
            appealDate,
            notes);
    }


    // =========================================================
    // Update Details
    // =========================================================

    public Result<Updated> UpdateDetails(
        string initiator,
        Guid appealTypeId,
        DateOnly appealDate,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(
            initiator))
        {
            return AppealErrors
                .InitiatorRequired;
        }


        if (appealTypeId == Guid.Empty)
        {
            return AppealErrors
                .AppealTypeRequired;
        }


        Initiator =
            initiator.Trim();

        AppealTypeId =
            appealTypeId;

        AppealDate =
            appealDate;

        Notes =
            Normalize(
                notes);


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


        var requirement =
            new AppealRequirement(
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
            return AppealRequirementErrors
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
            return AppealRequirementErrors
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
            return AppealRequirementErrors
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
            return AppealRequirementErrors
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
            return AppealRequirementErrors
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
            return AppealRequirementErrors
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

    private AppealRequirement?
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