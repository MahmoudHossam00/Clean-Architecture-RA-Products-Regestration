using ClosedXML.Excel;
using RAProductsRegestration.Application.Common
    .Interfaces.Files;
using RAProductsRegestration.Application.Features
    .Employees.Import;

namespace RAProductsRegestration.Infrastructure.Files;

internal sealed class EmployeeExcelReader
    : IEmployeeExcelReader
{
    public Task<EmployeeExcelRow[]> ReadAsync(
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        using var workbook =
            new XLWorkbook(
                stream);


        var worksheet =
            workbook.Worksheets
                .First();


        var headerRow =
            worksheet.Row(1);


        var columns =
            BuildColumnMap(
                headerRow);


        ValidateRequiredColumns(
            columns);


        var lastRow =
            worksheet.LastRowUsed();


        if (lastRow is null)
        {
            return Task.FromResult<
                EmployeeExcelRow[]>(
                []);
        }


        var result =
            new List<EmployeeExcelRow>();


        for (
            var rowNumber = 2;
            rowNumber <= lastRow.RowNumber();
            rowNumber++)
        {
            cancellationToken
                .ThrowIfCancellationRequested();


            var row =
                worksheet.Row(
                    rowNumber);


            var zenId =
                GetValue(
                    row,
                    columns,
                    "ZenId");


            var employeeNumber =
                GetValue(
                    row,
                    columns,
                    "EmployeeNumber");


            var nameEn =
                GetValue(
                    row,
                    columns,
                    "NameEn");


            var nameAr =
                GetValue(
                    row,
                    columns,
                    "NameAr");


            var email =
                GetValue(
                    row,
                    columns,
                    "Email");


            var departmentId =
                GetValue(
                    row,
                    columns,
                    "DepartmentId");


            // =================================================
            // Ignore completely empty rows
            // =================================================

            if (
                string.IsNullOrWhiteSpace(
                    zenId) &&

                string.IsNullOrWhiteSpace(
                    employeeNumber) &&

                string.IsNullOrWhiteSpace(
                    nameEn) &&

                string.IsNullOrWhiteSpace(
                    nameAr) &&

                string.IsNullOrWhiteSpace(
                    email) &&

                string.IsNullOrWhiteSpace(
                    departmentId))
            {
                continue;
            }


            result.Add(
                new EmployeeExcelRow(
                    rowNumber,
                    zenId,
                    employeeNumber,
                    nameEn,
                    nameAr,
                    email,
                    departmentId));
        }


        return Task.FromResult(
            result.ToArray());
    }


    private static Dictionary<string, int>
        BuildColumnMap(
            IXLRow headerRow)
    {
        var result =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);


        foreach (
            var cell
            in headerRow.CellsUsed())
        {
            var header =
                cell
                    .GetString()
                    .Trim();


            if (
                string.IsNullOrWhiteSpace(
                    header))
            {
                continue;
            }


            result[
                header] =
                cell.Address.ColumnNumber;
        }


        return result;
    }


    private static void ValidateRequiredColumns(
        Dictionary<string, int> columns)
    {
        var requiredColumns =
            new[]
            {
                "ZenId",
                "EmployeeNumber",
                "NameEn"
            };


        var missing =
            requiredColumns
                .Where(
                    x =>
                        !columns.ContainsKey(
                            x))
                .ToArray();


        if (
            missing.Length > 0)
        {
            throw new InvalidOperationException(
                "Missing required Excel column(s): " +
                string.Join(
                    ", ",
                    missing));
        }
    }


    private static string? GetValue(
        IXLRow row,
        Dictionary<string, int> columns,
        string columnName)
    {
        // Optional column doesn't exist.
        if (
            !columns.TryGetValue(
                columnName,
                out var columnNumber))
        {
            return null;
        }


        var value =
            row.Cell(
                    columnNumber)
                .GetFormattedString()
                .Trim();


        return string.IsNullOrWhiteSpace(
            value)
            ? null
            : value;
    }
}