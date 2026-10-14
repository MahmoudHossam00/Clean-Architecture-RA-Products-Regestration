using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.StorageConditions;

public static class StorageConditionErrors
{
    public static Error NameRequired =>
        Error.Validation(
            "StorageCondition.Name.Required",
            "Storage condition name is required.");
}