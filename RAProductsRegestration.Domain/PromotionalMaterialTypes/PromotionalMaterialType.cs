using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.PromotionalMaterialTypes;

public sealed class PromotionalMaterialType
    : AuditableEntity,
      IAggregateRoot
{
    // =====================================================
    // Constructors
    // =====================================================

    private PromotionalMaterialType()
    {
    }


    private PromotionalMaterialType(
        string name)
        : base(Guid.NewGuid())
    {
        Name = name;
    }


    // =====================================================
    // Properties
    // =====================================================

    public string Name { get; private set; } =
        null!;


    // =====================================================
    // Create
    // =====================================================

    public static Result<PromotionalMaterialType>
        Create(
            string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return PromotionalMaterialTypeErrors
                .NameRequired;
        }


        var type =
            new PromotionalMaterialType(
                name.Trim());


        return type;
    }


    // =====================================================
    // Rename
    // =====================================================

    public Result<Updated> Rename(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return PromotionalMaterialTypeErrors
                .NameRequired;
        }


        Name =
            name.Trim();


        return Result.Updated;
    }
}