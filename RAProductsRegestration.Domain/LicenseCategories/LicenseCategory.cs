using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.LicenseCategories;

public sealed class LicenseCategory
    : AuditableEntity, IAggregateRoot
{
    private LicenseCategory()
    {
    }

    private LicenseCategory(
        string name)
        : base(Guid.NewGuid())
    {
        Name = name;
    }


    public string Name
    {
        get;
        private set;
    } = null!;


    public static Result<LicenseCategory> Create(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return LicenseCategoryErrors
                .NameRequired;
        }

        return new LicenseCategory(
            name.Trim());
    }


    public Result<Updated> Rename(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return LicenseCategoryErrors
                .NameRequired;
        }

        Name =
            name.Trim();

        return Result.Updated;
    }
}