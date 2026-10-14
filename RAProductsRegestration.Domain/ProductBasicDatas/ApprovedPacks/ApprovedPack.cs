using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.ProductBasicDatas.ProductActiveIngredients;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedPacks;

public sealed class ApprovedPack : Entity
{
    private ApprovedPack()
    {
    }

    private ApprovedPack(
        string description,
        DatedAttachment approval)
        : base(Guid.NewGuid())
    {
        Description = description;
        Approval = approval;
    }

    public string Description { get; private set; } = null!;

    public DatedAttachment Approval { get; private set; } = null!;

    internal static Result<ApprovedPack> Create(
        string description,
        DatedAttachment? approval)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return ApprovedPackErrors.DescriptionRequired;
        }

        if (approval is null)
        {
            return ApprovedPackErrors.ApprovalRequired;
        }

        return new ApprovedPack(
            description.Trim(),
            approval);
    }

    internal Result<Updated> Update(
        string description,
        DatedAttachment? approval)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return ApprovedPackErrors.DescriptionRequired;
        }

        if (approval is null)
        {
            return ApprovedPackErrors.ApprovalRequired;
        }

        Description = description.Trim();
        Approval = approval;

        return Result.Updated;
    }

}