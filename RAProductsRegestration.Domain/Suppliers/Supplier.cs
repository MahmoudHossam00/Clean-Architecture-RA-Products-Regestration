using RAProductsRegestration.Domain.Common;

public sealed class Supplier : Entity, IAggregateRoot
{
    private Supplier()
    {
    }

    public Supplier(
        long jdeCode,
        string name)
        : base(Guid.NewGuid())
    {
        JdeCode = jdeCode;
        Name = name.Trim();
    }

    public long JdeCode { get; private set; }

    public string Name { get; private set; } = null!;

    public void UpdateFromJde(
        string name)
    {
        var normalizedName = name.Trim();

        if (Name == normalizedName)
        {
            return;
        }

        Name = normalizedName;
    }
}