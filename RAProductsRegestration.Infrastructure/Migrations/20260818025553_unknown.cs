using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class unknown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TradeName",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Decree600_Notes",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PV_Notes",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductActiveIngredient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Approval_Attachment_Notes",
                table: "ApprovedPack",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatedAttachment_Attachment_Notes",
                table: "ApprovedManufacturer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decree600_Notes",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_Notes",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_Notes",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "Approval_Attachment_Notes",
                table: "ApprovedPack");

            migrationBuilder.DropColumn(
                name: "DatedAttachment_Attachment_Notes",
                table: "ApprovedManufacturer");

            migrationBuilder.AlterColumn<string>(
                name: "TradeName",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
