using RAProductsRegestration.Domain.Common;

namespace RAProductsRegestration.Domain.Employees;

public sealed class Employee
    : Entity, IAggregateRoot
{
    private Employee()
    {
    }


    private Employee(
        Guid id,
        long zenId,
        int employeeNumber,
        string nameEn,
        string? nameAr,
        string? email,
        Guid? departmentId)
        : base(id)
    {
        ZenId =
            zenId;

        EmployeeNumber =
            employeeNumber;

        NameEn =
            nameEn.Trim();

        NameAr =
            Normalize(nameAr);

        Email =
            Normalize(email);

        DepartmentId =
            departmentId;
    }


    public long ZenId
    {
        get;
        private set;
    }


    public int EmployeeNumber
    {
        get;
        private set;
    }


    public string NameEn
    {
        get;
        private set;
    } = null!;


    public string? NameAr
    {
        get;
        private set;
    }


    public string? Email
    {
        get;
        private set;
    }


    public Guid? DepartmentId
    {
        get;
        private set;
    }


    // =========================================================
    // Create
    // =========================================================

    public static Employee Create(
        long zenId,
        int employeeNumber,
        string nameEn,
        string? nameAr,
        string? email,
        Guid? departmentId)
    {
        if (zenId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(zenId));
        }


        if (employeeNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(employeeNumber));
        }


        if (
            string.IsNullOrWhiteSpace(
                nameEn))
        {
            throw new ArgumentException(
                "Employee English name is required.",
                nameof(nameEn));
        }


        return new Employee(
            Guid.NewGuid(),
            zenId,
            employeeNumber,
            nameEn,
            nameAr,
            email,
            departmentId);
    }


    // =========================================================
    // Excel / external synchronization
    // =========================================================

    public void UpdateFromImport(
        int employeeNumber,
        string nameEn,
        string? nameAr,
        string? email,
        Guid? departmentId)
    {
        if (employeeNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(employeeNumber));
        }


        if (
            string.IsNullOrWhiteSpace(
                nameEn))
        {
            throw new ArgumentException(
                "Employee English name is required.",
                nameof(nameEn));
        }


        EmployeeNumber =
            employeeNumber;

        NameEn =
            nameEn.Trim();

        NameAr =
            Normalize(nameAr);

        Email =
            Normalize(email);

        DepartmentId =
            departmentId;
    }


    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(
            value)
            ? null
            : value.Trim();
    }
}