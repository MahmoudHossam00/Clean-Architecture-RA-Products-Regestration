using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.Departments;

public sealed class Department
    : AuditableEntity, IAggregateRoot
{
    private Department()
    {
    }


    private Department(

        string name, int zenId)
        : base(Guid.NewGuid())
    {
        Name =
            name;
        ZenId = zenId;
    }


    public string Name
    {
        get;
        private set;
    } = null!;
    public long ZenId
    {
        get;
        private set;
    } 


    // =========================================================
    // Create
    // =========================================================

    public static Result<Department> Create(
        string name,int zenId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DepartmentErrors
                .NameRequired;
        }


        return new Department(
            name.Trim(),zenId);
    }


    // =========================================================
    // Rename
    // =========================================================

    public Result<Updated> Rename(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DepartmentErrors
                .NameRequired;
        }


        Name =
            name.Trim();


        return Result.Updated;
    }
}