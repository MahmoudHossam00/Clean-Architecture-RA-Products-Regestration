using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedManufacturer_ProductBasicData_ProductBasicDataId",
                table: "ApprovedManufacturer");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedPack");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPack_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPack");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPrice_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedSupplier_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedSupplier");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductActiveIngredient_ProductBasicData_ProductBasicDataId",
                table: "ProductActiveIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductActiveIngredient",
                table: "ProductActiveIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApprovedSupplier",
                table: "ApprovedSupplier");

            migrationBuilder.RenameTable(
                name: "ProductActiveIngredient",
                newName: "ProductActiveIngredients");

            migrationBuilder.RenameTable(
                name: "ApprovedSupplier",
                newName: "ApprovedSuppliers");

            migrationBuilder.RenameIndex(
                name: "IX_ProductActiveIngredient_ProductBasicDataId",
                table: "ProductActiveIngredients",
                newName: "IX_ProductActiveIngredients_ProductBasicDataId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovedSupplier_ProductActiveIngredientId",
                table: "ApprovedSuppliers",
                newName: "IX_ApprovedSuppliers_ProductActiveIngredientId");

            migrationBuilder.AlterColumn<string>(
                name: "TradeName",
                table: "ProductBasicData",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhysicalDescription",
                table: "ProductBasicData",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Strength",
                table: "ProductActiveIngredients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhysicalDescription",
                table: "ProductActiveIngredients",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductActiveIngredients",
                table: "ProductActiveIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApprovedSuppliers",
                table: "ApprovedSuppliers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DosageForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosageForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageConditions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasicData_DosageFormId",
                table: "ProductBasicData",
                column: "DosageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasicData_ProductId",
                table: "ProductBasicData",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasicData_StorageConditionId",
                table: "ProductBasicData",
                column: "StorageConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductActiveIngredients_StorageConditionId",
                table: "ProductActiveIngredients",
                column: "StorageConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedSuppliers_SupplierId",
                table: "ApprovedSuppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DosageForms_Name",
                table: "DosageForms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageConditions_Name",
                table: "StorageConditions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedManufacturer_ProductBasicData_ProductBasicDataId",
                table: "ApprovedManufacturer",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredients_ProductActiveIngredientId",
                table: "ApprovedPack",
                column: "ProductActiveIngredientId",
                principalTable: "ProductActiveIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPack_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPack",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPrice_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPrice",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedSuppliers_ProductActiveIngredients_ProductActiveIngredientId",
                table: "ApprovedSuppliers",
                column: "ProductActiveIngredientId",
                principalTable: "ProductActiveIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedSuppliers_Suppliers_SupplierId",
                table: "ApprovedSuppliers",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductActiveIngredients_ProductBasicData_ProductBasicDataId",
                table: "ProductActiveIngredients",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductActiveIngredients_StorageConditions_StorageConditionId",
                table: "ProductActiveIngredients",
                column: "StorageConditionId",
                principalTable: "StorageConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBasicData_DosageForms_DosageFormId",
                table: "ProductBasicData",
                column: "DosageFormId",
                principalTable: "DosageForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBasicData_Products_ProductId",
                table: "ProductBasicData",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBasicData_StorageConditions_StorageConditionId",
                table: "ProductBasicData",
                column: "StorageConditionId",
                principalTable: "StorageConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedManufacturer_ProductBasicData_ProductBasicDataId",
                table: "ApprovedManufacturer");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredients_ProductActiveIngredientId",
                table: "ApprovedPack");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPack_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPack");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedPrice_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedSuppliers_ProductActiveIngredients_ProductActiveIngredientId",
                table: "ApprovedSuppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedSuppliers_Suppliers_SupplierId",
                table: "ApprovedSuppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductActiveIngredients_ProductBasicData_ProductBasicDataId",
                table: "ProductActiveIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductActiveIngredients_StorageConditions_StorageConditionId",
                table: "ProductActiveIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBasicData_DosageForms_DosageFormId",
                table: "ProductBasicData");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBasicData_Products_ProductId",
                table: "ProductBasicData");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBasicData_StorageConditions_StorageConditionId",
                table: "ProductBasicData");

            migrationBuilder.DropTable(
                name: "DosageForms");

            migrationBuilder.DropTable(
                name: "StorageConditions");

            migrationBuilder.DropIndex(
                name: "IX_ProductBasicData_DosageFormId",
                table: "ProductBasicData");

            migrationBuilder.DropIndex(
                name: "IX_ProductBasicData_ProductId",
                table: "ProductBasicData");

            migrationBuilder.DropIndex(
                name: "IX_ProductBasicData_StorageConditionId",
                table: "ProductBasicData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductActiveIngredients",
                table: "ProductActiveIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ProductActiveIngredients_StorageConditionId",
                table: "ProductActiveIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApprovedSuppliers",
                table: "ApprovedSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_ApprovedSuppliers_SupplierId",
                table: "ApprovedSuppliers");

            migrationBuilder.RenameTable(
                name: "ProductActiveIngredients",
                newName: "ProductActiveIngredient");

            migrationBuilder.RenameTable(
                name: "ApprovedSuppliers",
                newName: "ApprovedSupplier");

            migrationBuilder.RenameIndex(
                name: "IX_ProductActiveIngredients_ProductBasicDataId",
                table: "ProductActiveIngredient",
                newName: "IX_ProductActiveIngredient_ProductBasicDataId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovedSuppliers_ProductActiveIngredientId",
                table: "ApprovedSupplier",
                newName: "IX_ApprovedSupplier_ProductActiveIngredientId");

            migrationBuilder.AlterColumn<string>(
                name: "TradeName",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhysicalDescription",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Strength",
                table: "ProductActiveIngredient",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "PhysicalDescription",
                table: "ProductActiveIngredient",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductActiveIngredient",
                table: "ProductActiveIngredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApprovedSupplier",
                table: "ApprovedSupplier",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedManufacturer_ProductBasicData_ProductBasicDataId",
                table: "ApprovedManufacturer",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPack_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedPack",
                column: "ProductActiveIngredientId",
                principalTable: "ProductActiveIngredient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPack_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPack",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedPrice_ProductBasicData_ProductBasicDataId",
                table: "ApprovedPrice",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedSupplier_ProductActiveIngredient_ProductActiveIngredientId",
                table: "ApprovedSupplier",
                column: "ProductActiveIngredientId",
                principalTable: "ProductActiveIngredient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductActiveIngredient_ProductBasicData_ProductBasicDataId",
                table: "ProductActiveIngredient",
                column: "ProductBasicDataId",
                principalTable: "ProductBasicData",
                principalColumn: "Id");
        }
    }
}
