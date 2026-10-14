using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Departments;

public static class DepartmentErrors
{
    public static readonly Error CompanyIdRequired =
        Error.Validation(
            "Department.CompanyIdRequired",
            "Company ID is required.");


    public static readonly Error NameRequired =
        Error.Validation(
            "Department.NameRequired",
            "Department name is required.");


    public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "Department.NotFound",
            $"Department '{id}' was not found.");


    public static Error AlreadyExists(
        Guid companyId,
        string name) =>
        Error.Conflict(
            "Department.AlreadyExists",
            $"Department '{name}' already exists for company '{companyId}'.");
}