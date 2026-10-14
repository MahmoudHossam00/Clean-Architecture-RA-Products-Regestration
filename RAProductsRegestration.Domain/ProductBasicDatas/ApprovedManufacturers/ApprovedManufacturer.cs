using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedManufacturers;

public sealed class ApprovedManufacturer : Entity
{
    private ApprovedManufacturer()
    {
    }

    private ApprovedManufacturer(
        string manufacturerName,
        DatedAttachment? att)
        : base(Guid.Empty)
    {
        ManufacturerName = manufacturerName;
        DatedAttachment = att;
        IsActive = true;
    }

    public string ManufacturerName { get; private set; } = null!;
    public DatedAttachment? DatedAttachment { get; private set; } = null;
    public bool IsActive { get; private set; }

    internal static Result<ApprovedManufacturer> Create(
        string manufacturerName,DatedAttachment ATT)
    {
        if (string.IsNullOrWhiteSpace(manufacturerName))
        {
            return ApprovedManufacturerErrors
                .NameRequired;
        }

        return new ApprovedManufacturer(
            manufacturerName.Trim(), ATT);
    }

    internal Result<Updated> Activate()
    {
        IsActive = true;

        return Result.Updated;
    }

    internal Result<Updated> Deactivate()
    {
        IsActive = false;

        return Result.Updated;
    }
}