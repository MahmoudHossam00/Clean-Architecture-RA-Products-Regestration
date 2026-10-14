using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.AppealTypes;

public sealed class AppealType
    : AuditableEntity, IAggregateRoot
{
    private AppealType()
    {
    }


    private AppealType(
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


    public static Result<AppealType> Create(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return AppealTypeErrors
                .NameRequired;
        }


        return new AppealType(
            name.Trim());
    }


    public Result<Updated> Rename(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return AppealTypeErrors
                .NameRequired;
        }


        Name =
            name.Trim();


        return Result.Updated;
    }
}