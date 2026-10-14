using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class suppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "ApprovedSupplier");

            migrationBuilder.RenameColumn(
                name: "StorageCondition",
                table: "ProductBasicData",
                newName: "PV_Description");

            migrationBuilder.RenameColumn(
                name: "DosageForm",
                table: "ProductBasicData",
                newName: "Decree600_Description");

            migrationBuilder.AddColumn<Guid>(
                name: "DosageFormId",
                table: "ProductBasicData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StorageConditionId",
                table: "ProductBasicData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradeName",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalDescription",
                table: "ProductActiveIngredient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductActiveIngredient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductActiveIngredient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ShelfLife_Date",
                table: "ProductActiveIngredient",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StorageConditionId",
                table: "ProductActiveIngredient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "ApprovedSupplier",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApprovedPack",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductActiveIngredientId",
                table: "ApprovedPack",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DatedAttachment_Attachment_AttachmentId",
                table: "ApprovedManufacturer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatedAttachment_Attachment_FileName",
                table: "ApprovedManufacturer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DatedAttachment_Date",
                table: "ApprovedManufacturer",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JdeCode = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedPack_ProductActiveIngredientId",
                table: "ApprovedPack",
                column: "ProductActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_JdeCode",
                table: "Suppliers",
                column: "JdeCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedPack",
                column: "ProductActiveIngredientId",
                principalTable: "ProductActiveIngredient",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedPack");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_ApprovedPack_ProductActiveIngredientId",
                table: "ApprovedPack");

            migrationBuilder.DropColumn(
                name: "DosageFormId",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "StorageConditionId",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "TradeName",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PhysicalDescription",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_AttachmentId",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Attachment_FileName",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "ShelfLife_Date",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "StorageConditionId",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ApprovedSupplier");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApprovedPack");

            migrationBuilder.DropColumn(
                name: "ProductActiveIngredientId",
                table: "ApprovedPack");

            migrationBuilder.DropColumn(
                name: "DatedAttachment_Attachment_AttachmentId",
                table: "ApprovedManufacturer");

            migrationBuilder.DropColumn(
                name: "DatedAttachment_Attachment_FileName",
                table: "ApprovedManufacturer");

            migrationBuilder.DropColumn(
                name: "DatedAttachment_Date",
                table: "ApprovedManufacturer");

            migrationBuilder.RenameColumn(
                name: "PV_Description",
                table: "ProductBasicData",
                newName: "StorageCondition");

            migrationBuilder.RenameColumn(
                name: "Decree600_Description",
                table: "ProductBasicData",
                newName: "DosageForm");

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "ApprovedSupplier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
