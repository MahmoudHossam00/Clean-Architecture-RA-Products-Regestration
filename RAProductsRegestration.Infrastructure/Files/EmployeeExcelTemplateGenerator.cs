using ClosedXML.Excel;
using RAProductsRegestration.Application.Common
    .Interfaces.Files;

namespace RAProductsRegestration.Infrastructure.Files;

internal sealed class EmployeeExcelTemplateGenerator
    : IEmployeeExcelTemplateGenerator
{
    public byte[] Generate()
    {
        using var workbook =
            new XLWorkbook();


        var worksheet =
            workbook.Worksheets.Add(
                "Employees");


        // =====================================================
        // Headers
        // =====================================================

        worksheet.Cell(1, 1)
            .Value =
            "ZenId";

        worksheet.Cell(1, 2)
            .Value =
            "EmployeeNumber";

        worksheet.Cell(1, 3)
            .Value =
            "NameEn";

        worksheet.Cell(1, 4)
            .Value =
            "NameAr";

        worksheet.Cell(1, 5)
            .Value =
            "Email";

        worksheet.Cell(1, 6)
            .Value =
            "DepartmentId";


        var headerRange =
            worksheet.Range(
                1,
                1,
                1,
                6);


        headerRange.Style.Font.Bold =
            true;


        // =====================================================
        // Example row
        // =====================================================

        worksheet.Cell(2, 1)
            .Value =
            684741;

        worksheet.Cell(2, 2)
            .Value =
            3358;

        worksheet.Cell(2, 3)
            .Value =
            "Example Employee";

        worksheet.Cell(2, 4)
            .Value =
            "موظف تجريبي";

        worksheet.Cell(2, 5)
            .Value =
            "employee@example.com";

        worksheet.Cell(2, 6)
            .Value =
            "";


        // =====================================================
        // Instructions sheet
        // =====================================================

        var instructions =
            workbook.Worksheets.Add(
                "Instructions");


        instructions.Cell(1, 1)
            .Value =
            "Column";

        instructions.Cell(1, 2)
            .Value =
            "Required";

        instructions.Cell(1, 3)
            .Value =
            "Description";


        instructions.Cell(2, 1)
            .Value =
            "ZenId";

        instructions.Cell(2, 2)
            .Value =
            "Yes";

        instructions.Cell(2, 3)
            .Value =
            "ZenHR employee identifier. Used as the upsert key.";


        instructions.Cell(3, 1)
            .Value =
            "EmployeeNumber";

        instructions.Cell(3, 2)
            .Value =
            "Yes";

        instructions.Cell(3, 3)
            .Value =
            "Unique employee number.";


        instructions.Cell(4, 1)
            .Value =
            "NameEn";

        instructions.Cell(4, 2)
            .Value =
            "Yes";

        instructions.Cell(4, 3)
            .Value =
            "Employee English name.";


        instructions.Cell(5, 1)
            .Value =
            "NameAr";

        instructions.Cell(5, 2)
            .Value =
            "No";

        instructions.Cell(5, 3)
            .Value =
            "Employee Arabic name.";


        instructions.Cell(6, 1)
            .Value =
            "Email";

        instructions.Cell(6, 2)
            .Value =
            "No";

        instructions.Cell(6, 3)
            .Value =
            "Employee email address.";


        instructions.Cell(7, 1)
            .Value =
            "DepartmentId";

        instructions.Cell(7, 2)
            .Value =
            "No";

        instructions.Cell(7, 3)
            .Value =
            "Existing Department Id. Leave blank if the employee has no department.";


        instructions.Cell(9, 1)
            .Value =
            "Upsert behavior";


        instructions.Cell(10, 1)
            .Value =
            "Existing ZenId";

        instructions.Cell(10, 2)
            .Value =
            "Employee will be updated.";


        instructions.Cell(11, 1)
            .Value =
            "New ZenId";

        instructions.Cell(11, 2)
            .Value =
            "Employee will be inserted.";


        instructions.Cell(12, 1)
            .Value =
            "Duplicate EmployeeNumber belonging to another ZenId";

        instructions.Cell(12, 2)
            .Value =
            "Row will be skipped.";


        instructions.Cell(13, 1)
            .Value =
            "Invalid DepartmentId";

        instructions.Cell(13, 2)
            .Value =
            "Row will be skipped.";


        // =====================================================
        // Formatting
        // =====================================================

        worksheet.SheetView
            .FreezeRows(
                1);

        worksheet.Columns()
            .AdjustToContents();


        instructions.SheetView
            .FreezeRows(
                1);

        instructions.Columns()
            .AdjustToContents();


        // Leave enough room for normal values.
        worksheet.Column(3).Width =
            Math.Max(
                worksheet.Column(3).Width,
                25);

        worksheet.Column(4).Width =
            Math.Max(
                worksheet.Column(4).Width,
                25);

        worksheet.Column(5).Width =
            Math.Max(
                worksheet.Column(5).Width,
                30);

        worksheet.Column(6).Width =
            Math.Max(
                worksheet.Column(6).Width,
                40);


        // =====================================================
        // Export
        // =====================================================

        using var stream =
            new MemoryStream();


        workbook.SaveAs(
            stream);


        return stream.ToArray();
    }
}