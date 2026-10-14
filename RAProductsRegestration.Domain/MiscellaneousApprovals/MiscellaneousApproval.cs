using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.MiscellaneousApprovals.Requirements;

namespace RAProductsRegestration.Domain.MiscellaneousApprovals;

public sealed class MiscellaneousApproval
    : AuditableEntity,
      IAggregateRoot
{
    private readonly List<ApprovalRequirement>
        _approvalRequirements = [];


    private MiscellaneousApproval()
    {
    }


    private MiscellaneousApproval(
        Guid productId,
        Guid fileTypeId,
        string initiator,
        DateOnly submissionDate)
        : base(Guid.NewGuid())
    {
        ProductId =
            productId;

        FileTypeId =
            fileTypeId;

        Initiator =
            initiator;

        SubmissionDate =
            submissionDate;
    }


    // =====================================================
    // Properties
    // =====================================================

    public Guid ProductId
    {
        get;
        private set;
    }


    public Guid FileTypeId
    {
        get;
        private set;
    }


    public string Initiator
    {
        get;
        private set;
    } = null!;


    public DateOnly SubmissionDate
    {
        get;
        private set;
    }


    // =====================================================
    // Approval
    // =====================================================

    public DateOnly? ApprovalDate
    {
        get;
        private set;
    }


    public DateOnly? ApprovalExpiryDate
    {
        get;
        private set;
    }


    // =====================================================
    // Requirements
    // =====================================================

    public IReadOnlyCollection<ApprovalRequirement>
        ApprovalRequirements =>
            _approvalRequirements
                .AsReadOnly();


    // =====================================================
    // Create
    // =====================================================

    public static Result<MiscellaneousApproval>
        Create(
            Guid productId,
            Guid fileTypeId,
            string initiator,
            DateOnly submissionDate)
    {
        if (productId == Guid.Empty)
        {
            return MiscellaneousApprovalErrors
                .ProductIdRequired;
        }


        if (fileTypeId == Guid.Empty)
        {
            return MiscellaneousApprovalErrors
                .FileTypeRequired;
        }


        if (string.IsNullOrWhiteSpace(
                initiator))
        {
            return MiscellaneousApprovalErrors
                .InitiatorRequired;
        }


        return new MiscellaneousApproval(
            productId,
            fileTypeId,
            initiator.Trim(),
            submissionDate);
    }


    // =====================================================
    // Update File Type
    // =====================================================

    public Result<Updated> UpdateFileType(
        Guid fileTypeId)
    {
        if (fileTypeId == Guid.Empty)
        {
            return MiscellaneousApprovalErrors
                .FileTypeRequired;
        }


        FileTypeId =
            fileTypeId;


        return Result.Updated;
    }


    // =====================================================
    // Update Details
    // =====================================================

    public Result<Updated> UpdateDetails(
        string initiator,
        DateOnly submissionDate)
    {
        if (string.IsNullOrWhiteSpace(
                initiator))
        {
            return MiscellaneousApprovalErrors
                .InitiatorRequired;
        }


        Initiator =
            initiator.Trim();

        SubmissionDate =
            submissionDate;


        return Result.Updated;
    }


    // =====================================================
    // Approval
    // =====================================================

    public Result<Updated> SetApproval(
        DateOnly approvalDate,
        DateOnly approvalExpiryDate)
    {
        if (
            approvalDate <
            SubmissionDate
        )
        {
            return MiscellaneousApprovalErrors
                .ApprovalBeforeSubmission;
        }


        if (
            approvalExpiryDate <
            approvalDate
        )
        {
            return MiscellaneousApprovalErrors
                .ApprovalExpiryBeforeApprovalDate;
        }


        ApprovalDate =
            approvalDate;

        ApprovalExpiryDate =
            approvalExpiryDate;


        return Result.Updated;
    }


    public Result<Updated>
        RemoveApproval()
    {
        ApprovalDate =
            null;

        ApprovalExpiryDate =
            null;


        return Result.Updated;
    }


    // =====================================================
    // Add Requirement
    // =====================================================

    public Result<Created>
        AddApprovalRequirement(
            string name,
            DateOnly deadline,
            Guid responsibleDepartmentId,
            AttachmentReference?
                requirementAttachment,
            string? notes)
    {
        if (string.IsNullOrWhiteSpace(
                name))
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


        var requirement =
            new ApprovalRequirement(
                name.Trim(),
                deadline,
                responsibleDepartmentId,
                requirementAttachment,
                notes);


        _approvalRequirements.Add(
            requirement);


        return Result.Created;
    }


    // =====================================================
    // Update Requirement
    // =====================================================

    public Result<Updated>
        UpdateApprovalRequirement(
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
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement.Update(
            name,
            deadline,
            responsibleDepartmentId,
            notes);
    }


    // =====================================================
    // Requirement Attachment
    // =====================================================

    public Result<Updated>
        SetApprovalRequirementAttachment(
            Guid requirementId,
            AttachmentReference attachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .SetRequirementAttachment(
                attachment);
    }


    public Result<Updated>
        RemoveApprovalRequirementAttachment(
            Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveRequirementAttachment();
    }


    // =====================================================
    // Approve Requirement
    // =====================================================

    public Result<Updated>
        ApproveRequirement(
            Guid requirementId,
            DateOnly approvalDate,
            AttachmentReference approvalAttachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement.Approve(
            approvalDate,
            approvalAttachment);
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
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveApproval();
    }


    // =====================================================
    // Remove Requirement
    // =====================================================

    public Result<Deleted>
        RemoveApprovalRequirement(
            Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return ApprovalRequirementErrors
                .NotFound(
                    requirementId);
        }


        _approvalRequirements.Remove(
            requirement);


        return Result.Deleted;
    }


    // =====================================================
    // Helper
    // =====================================================

    private ApprovalRequirement?
        FindRequirement(
            Guid requirementId)
    {
        return _approvalRequirements
            .FirstOrDefault(
                x =>
                    x.Id ==
                    requirementId);
    }
}