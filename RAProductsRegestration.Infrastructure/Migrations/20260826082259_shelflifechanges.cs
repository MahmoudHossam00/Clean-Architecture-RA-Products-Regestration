using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class shelflifechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Date",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductActiveIngredients");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductActiveIngredients");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Date",
                table: "ProductActiveIngredients");

            migrationBuilder.RenameColumn(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductBasicData",
                newName: "ShelfLife");

            migrationBuilder.RenameColumn(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductActiveIngredients",
                newName: "ShelfLife");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShelfLife",
                table: "ProductBasicData",
                newName: "ShelfLife_Attachment_Notes");

            migrationBuilder.RenameColumn(
                name: "ShelfLife",
                table: "ProductActiveIngredients",
                newName: "ShelfLife_Attachment_Notes");

            migrationBuilder.AddColumn<Guid>(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductBasicData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ShelfLife_Date",
                table: "ProductBasicData",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductActiveIngredients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductActiveIngredients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ShelfLife_Date",
                table: "ProductActiveIngredients",
                type: "date",
                nullable: true);
        }
    }
}
