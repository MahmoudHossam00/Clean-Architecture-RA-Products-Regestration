using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;

public sealed class MiscellaneousApprovalFileType
    : AuditableEntity,
      IAggregateRoot
{
    private MiscellaneousApprovalFileType()
    {
    }


    private MiscellaneousApprovalFileType(
        string name)
        : base(Guid.NewGuid())
    {
        Name = name;
    }


    public string Name { get; private set; } =
        null!;


    public static Result<MiscellaneousApprovalFileType>
        Create(
            string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return MiscellaneousApprovalFileTypeErrors
                .NameRequired;
        }


        return new MiscellaneousApprovalFileType(
            name.Trim());
    }


    public Result<Updated> Rename(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return MiscellaneousApprovalFileTypeErrors
                .NameRequired;
        }


        Name =
            name.Trim();


        return Result.Updated;
    }
}