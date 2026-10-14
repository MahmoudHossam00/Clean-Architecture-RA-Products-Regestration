using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;

namespace RAProductsRegestration.Domain.PromotionalMaterials;

public sealed class PromotionalMaterial
    : AuditableEntity,
      IAggregateRoot
{
    // =====================================================
    // Constructor
    // =====================================================

    private PromotionalMaterial()
    {
    }


    private PromotionalMaterial(
        Guid productId,
        Guid promotionalMaterialTypeId,
        AttachmentReference materialFile)
        : base(Guid.NewGuid())
    {
        ProductId =
            productId;

        PromotionalMaterialTypeId =
            promotionalMaterialTypeId;

        MaterialFile =
            materialFile;
    }


    // =====================================================
    // Properties
    // =====================================================

    public Guid ProductId { get; private set; }


    public Guid PromotionalMaterialTypeId
    {
        get;
        private set;
    }


    // =====================================================
    // Promotional Material File
    // =====================================================

    public AttachmentReference MaterialFile
    {
        get;
        private set;
    } = null!;


    // =====================================================
    // Approval
    // =====================================================

    public DatedAttachment? Approval
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
    // Create
    // =====================================================

    public static Result<PromotionalMaterial>
        Create(
            Guid productId,
            Guid promotionalMaterialTypeId,
            AttachmentReference materialFile)
    {
        if (productId == Guid.Empty)
        {
            return PromotionalMaterialErrors
                .ProductIdRequired;
        }


        if (
            promotionalMaterialTypeId ==
            Guid.Empty
        )
        {
            return PromotionalMaterialErrors
                .PromotionalMaterialTypeRequired;
        }


        ArgumentNullException
            .ThrowIfNull(
                materialFile);


        var promotionalMaterial =
            new PromotionalMaterial(
                productId,
                promotionalMaterialTypeId,
                materialFile);


        return promotionalMaterial;
    }


    // =====================================================
    // Update Type
    // =====================================================

    public Result<Updated>
        UpdateType(
            Guid promotionalMaterialTypeId)
    {
        if (
            promotionalMaterialTypeId ==
            Guid.Empty
        )
        {
            return PromotionalMaterialErrors
                .PromotionalMaterialTypeRequired;
        }


        PromotionalMaterialTypeId =
            promotionalMaterialTypeId;


        return Result.Updated;
    }


    // =====================================================
    // Replace Material File
    // =====================================================

    public Result<Updated>
        ReplaceMaterialFile(
            AttachmentReference materialFile)
    {
        ArgumentNullException
            .ThrowIfNull(
                materialFile);


        MaterialFile =
            materialFile;


        return Result.Updated;
    }


    // =====================================================
    // Set Approval
    // =====================================================

    public Result<Updated>
        SetApproval(
            DateOnly approvalDate,
            DateOnly approvalExpiryDate,
            AttachmentReference approvalAttachment)
    {
        ArgumentNullException
            .ThrowIfNull(
                approvalAttachment);


        if (
            approvalExpiryDate <
            approvalDate
        )
        {
            return PromotionalMaterialErrors
                .ApprovalExpiryBeforeApprovalDate;
        }


        Approval =
            new DatedAttachment(
                approvalDate,
                approvalAttachment);


        ApprovalExpiryDate =
            approvalExpiryDate;


        return Result.Updated;
    }


    // =====================================================
    // Remove Approval
    // =====================================================

    public Result<Updated>
        RemoveApproval()
    {
        Approval =
            null;


        ApprovalExpiryDate =
            null;


        return Result.Updated;
    }
}