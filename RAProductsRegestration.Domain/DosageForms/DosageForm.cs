using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.DosageForms;

public sealed class DosageForm
    : AuditableEntity, IAggregateRoot
{
    private DosageForm()
    {
    }

    private DosageForm(
        string name)
        : base(Guid.NewGuid())
    {
        Name = name;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public static Result<DosageForm> Create(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DosageFormErrors.NameRequired;
        }

        return new DosageForm(
            name.Trim());
    }

    public Result<Updated> UpdateName(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DosageFormErrors.NameRequired;
        }

        Name = name.Trim();

        return Result.Updated;
    }

    public Result<Updated> Activate()
    {
        IsActive = true;

        return Result.Updated;
    }

    public Result<Updated> Deactivate()
    {
        IsActive = false;

        return Result.Updated;
    }
}