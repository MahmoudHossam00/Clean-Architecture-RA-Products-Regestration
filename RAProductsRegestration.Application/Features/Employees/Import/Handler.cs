using RAProductsRegestration.Application.Common
    .Interfaces.Files;
using RAProductsRegestration.Application.Common
    .Interfaces.Messaging;
using RAProductsRegestration.Application.Common
    .Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.Employees;

namespace RAProductsRegestration.Application.Features
    .Employees.Import;

internal sealed class Handler(
    IEmployeeExcelReader excelReader,
    IRepository<Employee> employeeRepository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        ImportEmployeesCommand,
        EmployeeImportResponse>
{
    public async Task<Result<EmployeeImportResponse>> Handle(
        ImportEmployeesCommand request,
        CancellationToken cancellationToken)
    {
        // =====================================================
        // Validate File
        // =====================================================

        var extension =
            Path.GetExtension(
                request.FileName);


        if (
            !string.Equals(
                extension,
                ".xlsx",
                StringComparison.OrdinalIgnoreCase))
        {
            return Error.Validation(
                "EmployeeImport.InvalidFileType",
                "Only .xlsx Excel files are supported.");
        }


        // =====================================================
        // Read Excel
        // =====================================================

        EmployeeExcelRow[] rows;


        try
        {
            rows =
                await excelReader.ReadAsync(
                    request.Content,
                    cancellationToken);
        }
        catch (Exception)
        {
            return Error.Validation(
                "EmployeeImport.InvalidExcel",
                "The Excel file could not be read.");
        }


        if (rows.Length == 0)
        {
            return Error.Validation(
                "EmployeeImport.EmptyFile",
                "The Excel file contains no employee rows.");
        }


        var errors =
            new List<EmployeeImportError>();


        var validRows =
            new List<ValidEmployeeRow>();


        var fileZenIds =
            new HashSet<long>();


        var fileEmployeeNumbers =
            new HashSet<int>();
        var existingdepts =
    await departmentRepository.WhereAsync(
        _ => true,
        cancellationToken);


        var departmentsByZenId =
            existingdepts.ToDictionary(
                x => x.ZenId,
                x => x.Id);

        // =====================================================
        // Parse + Validate Rows
        // =====================================================

        foreach (
            var row
            in rows)
        {
            // -------------------------------------------------
            // ZenId
            // -------------------------------------------------

            if (
                !long.TryParse(
                    row.ZenId,
                    out var zenId) ||
                zenId <= 0)
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        "ZenId must be a positive number."));

                continue;
            }


            // -------------------------------------------------
            // EmployeeNumber
            // -------------------------------------------------

            if (
                !int.TryParse(
                    row.EmployeeNumber,
                    out var employeeNumber) ||
                employeeNumber <= 0)
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        "EmployeeNumber must be a positive number."));

                continue;
            }


            // -------------------------------------------------
            // NameEn
            // -------------------------------------------------

            if (
                string.IsNullOrWhiteSpace(
                    row.NameEn))
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        "NameEn is required."));

                continue;
            }


            // -------------------------------------------------
            // DepartmentId
            // -------------------------------------------------

            Guid? departmentId =
    null;


            if (
                !string.IsNullOrWhiteSpace(
                    row.DepartmentId))
            {
                // Excel value is Department.ZenId
                if (
                    !int.TryParse(
                        row.DepartmentId,
                        out var departmentZenId) ||
                    departmentZenId <= 0)
                {
                    errors.Add(
                        new EmployeeImportError(
                            row.RowNumber,
                            "Department must be a valid Department ZenId."));

                    continue;
                }


                // Convert external ZenId -> internal Guid Id
                if (
                    !departmentsByZenId.TryGetValue(
                        departmentZenId,
                        out var foundDepartmentId))
                {
                    errors.Add(
                        new EmployeeImportError(
                            row.RowNumber,
                            $"Department with ZenId {departmentZenId} was not found."));

                    continue;
                }


                departmentId =
                    foundDepartmentId;
            }


            // -------------------------------------------------
            // Duplicate ZenId inside file
            // -------------------------------------------------

            if (
                !fileZenIds.Add(
                    zenId))
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        $"Duplicate ZenId '{zenId}' " +
                        "inside the Excel file."));

                continue;
            }


            // -------------------------------------------------
            // Duplicate EmployeeNumber inside file
            // -------------------------------------------------

            if (
                !fileEmployeeNumbers.Add(
                    employeeNumber))
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        $"Duplicate EmployeeNumber " +
                        $"'{employeeNumber}' " +
                        "inside the Excel file."));

                continue;
            }


            validRows.Add(
                new ValidEmployeeRow(
                    row.RowNumber,
                    zenId,
                    employeeNumber,
                    row.NameEn.Trim(),
                    Normalize(row.NameAr),
                    Normalize(row.Email),
                    departmentId));
        }


        if (validRows.Count == 0)
        {
            return new EmployeeImportResponse(
                rows.Length,
                0,
                0,
                rows.Length,
                errors.ToArray());
        }


        // =====================================================
        // Load Departments
        // =====================================================

        var departmentIds =
            validRows
                .Where(x =>
                    x.DepartmentId.HasValue)
                .Select(x =>
                    x.DepartmentId!.Value)
                .Distinct()
                .ToArray();


        IReadOnlyList<Department> departments =
            departmentIds.Length == 0
                ? []
                : await departmentRepository
                    .WhereAsync(
                        x =>
                            departmentIds.Contains(
                                x.Id),
                        cancellationToken);


        var existingDepartmentIds =
            departments
                .Select(x =>
                    x.Id)
                .ToHashSet();


        // =====================================================
        // Remove Rows With Invalid Departments
        // =====================================================

        var rowsWithValidDepartments =
            new List<ValidEmployeeRow>();


        foreach (
            var row
            in validRows)
        {
            if (
                row.DepartmentId.HasValue &&
                !existingDepartmentIds.Contains(
                    row.DepartmentId.Value))
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        $"Department " +
                        $"'{row.DepartmentId}' " +
                        "does not exist."));

                continue;
            }


            rowsWithValidDepartments.Add(
                row);
        }


        if (
            rowsWithValidDepartments.Count == 0)
        {
            return new EmployeeImportResponse(
                rows.Length,
                0,
                0,
                errors.Count,
                errors.ToArray());
        }


        // =====================================================
        // Load Existing Employees
        // =====================================================

        var zenIds =
            rowsWithValidDepartments
                .Select(x =>
                    x.ZenId)
                .ToArray();


        var employeeNumbers =
            rowsWithValidDepartments
                .Select(x =>
                    x.EmployeeNumber)
                .ToArray();


        var existingEmployees =
            await employeeRepository
                .WhereAsync(
                    x =>
                        zenIds.Contains(
                            x.ZenId) ||

                        employeeNumbers.Contains(
                            x.EmployeeNumber),

                    cancellationToken);


        var byZenId =
            existingEmployees
                .ToDictionary(
                    x =>
                        x.ZenId);


        var byEmployeeNumber =
            existingEmployees
                .ToDictionary(
                    x =>
                        x.EmployeeNumber);


        var inserted =
            0;


        var updated =
            0;


        // =====================================================
        // UPSERT
        // =====================================================

        foreach (
            var row
            in rowsWithValidDepartments)
        {
            // -------------------------------------------------
            // Existing Zen employee
            // -------------------------------------------------

            if (
                byZenId.TryGetValue(
                    row.ZenId,
                    out var existing))
            {
                // Employee number is being used
                // by another employee.
                if (
                    byEmployeeNumber.TryGetValue(
                        row.EmployeeNumber,
                        out var numberOwner) &&
                    numberOwner.Id !=
                        existing.Id)
                {
                    errors.Add(
                        new EmployeeImportError(
                            row.RowNumber,
                            $"EmployeeNumber " +
                            $"'{row.EmployeeNumber}' " +
                            "is already assigned to " +
                            "another employee."));

                    continue;
                }


                var oldEmployeeNumber =
                    existing.EmployeeNumber;


                existing.UpdateFromImport(
                    row.EmployeeNumber,
                    row.NameEn,
                    row.NameAr,
                    row.Email,
                    row.DepartmentId);


                // EmployeeNumber may have changed.
                if (
                    oldEmployeeNumber !=
                    existing.EmployeeNumber)
                {
                    byEmployeeNumber.Remove(
                        oldEmployeeNumber);

                    byEmployeeNumber[
                        existing.EmployeeNumber] =
                        existing;
                }


                updated++;

                continue;
            }


            // -------------------------------------------------
            // New ZenId but employee number already belongs
            // to another employee.
            // -------------------------------------------------

            if (
                byEmployeeNumber.ContainsKey(
                    row.EmployeeNumber))
            {
                errors.Add(
                    new EmployeeImportError(
                        row.RowNumber,
                        $"EmployeeNumber " +
                        $"'{row.EmployeeNumber}' " +
                        "already exists with another ZenId."));

                continue;
            }


            // -------------------------------------------------
            // Insert
            // -------------------------------------------------

            var employee =
                Employee.Create(
                    row.ZenId,
                    row.EmployeeNumber,
                    row.NameEn,
                    row.NameAr,
                    row.Email,
                    row.DepartmentId);


            employeeRepository.Add(
                employee);


            byZenId[
                employee.ZenId] =
                employee;


            byEmployeeNumber[
                employee.EmployeeNumber] =
                employee;


            inserted++;
        }


        // =====================================================
        // Save Once
        // =====================================================

        if (
            inserted > 0 ||
            updated > 0)
        {
            await unitOfWork
                .SaveChangesAsync(
                    cancellationToken);
        }


        return new EmployeeImportResponse(
            TotalRows:
                rows.Length,

            Inserted:
                inserted,

            Updated:
                updated,

            Skipped:
                errors.Count,

            Errors:
                errors.ToArray());
    }


    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(
            value)
            ? null
            : value.Trim();
    }


    private sealed record ValidEmployeeRow(
        int RowNumber,
        long ZenId,
        int EmployeeNumber,
        string NameEn,
        string? NameAr,
        string? Email,
        Guid? DepartmentId);
}