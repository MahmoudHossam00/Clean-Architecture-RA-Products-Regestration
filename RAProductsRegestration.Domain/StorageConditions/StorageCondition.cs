using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.StorageConditions;

public sealed class StorageCondition
    : AuditableEntity, IAggregateRoot
{
    private StorageCondition()
    {
    }

    private StorageCondition(
        string name)
        : base(Guid.NewGuid())
    {
        Name = name;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public static Result<StorageCondition> Create(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return StorageConditionErrors.NameRequired;
        }

        return new StorageCondition(
            name.Trim());
    }

    public Result<Updated> UpdateName(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return StorageConditionErrors.NameRequired;
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