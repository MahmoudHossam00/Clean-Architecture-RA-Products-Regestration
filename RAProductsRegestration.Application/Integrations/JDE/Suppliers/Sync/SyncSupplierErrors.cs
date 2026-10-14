using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Integrations
    .JDE.Suppliers.Sync;

internal static class SyncSuppliersErrors
{
    public static Error InvalidJdeCode(
        long code) =>
        Error.Validation(
            "JDE.Supplier.Code.Invalid",
            $"JDE supplier code '{code}' is invalid.");

    public static Error NameRequired(
        long code) =>
        Error.Validation(
            "JDE.Supplier.Name.Required",
            $"JDE supplier '{code}' does not have a valid name.");
}