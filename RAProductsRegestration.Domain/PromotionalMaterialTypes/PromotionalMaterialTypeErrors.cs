
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.PromotionalMaterialTypes;

public static class PromotionalMaterialTypeErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "PromotionalMaterialType.NameRequired",
            "Promotional material type name is required.");


    public static Error NotFound(
        Guid id)
    {
        return Error.NotFound(
            "PromotionalMaterialType.NotFound",
            $"Promotional material type '{id}' was not found.");
    }


    public static Error AlreadyExists(
        string name)
    {
        return Error.Conflict(
            "PromotionalMaterialType.AlreadyExists",
            $"Promotional material type '{name}' already exists.");
    }
}