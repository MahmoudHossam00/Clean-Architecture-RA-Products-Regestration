using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CTDId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductBasicData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DosageForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Decree600_Deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    Decree600_FulfillmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Decree600_IsFulfilled = table.Column<bool>(type: "bit", nullable: true),
                    PV_Deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    PV_FulfillmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PV_IsFulfilled = table.Column<bool>(type: "bit", nullable: true),
                    ShelfLife_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    ShelfLife_Attachment_AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShelfLife_Attachment_FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBasicData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CTDId = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovedManufacturer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufacturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedManufacturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovedManufacturer_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovedPack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approval_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Approval_Attachment_AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Approval_Attachment_FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedPack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovedPack_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovedPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovedPrice_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductActiveIngredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveIngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductActiveIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductActiveIngredient_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovedSupplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductActiveIngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovedSupplier_ProductActiveIngredient_ProductActiveIngredientId",
                        column: x => x.ProductActiveIngredientId,
                        principalTable: "ProductActiveIngredient",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_Code",
                table: "ActiveIngredients",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_CTDId",
                table: "ActiveIngredients",
                column: "CTDId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedManufacturer_ProductBasicDataId",
                table: "ApprovedManufacturer",
                column: "ProductBasicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedPack_ProductBasicDataId",
                table: "ApprovedPack",
                column: "ProductBasicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedPrice_ProductBasicDataId",
                table: "ApprovedPrice",
                column: "ProductBasicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedSupplier_ProductActiveIngredientId",
                table: "ApprovedSupplier",
                column: "ProductActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Code",
                table: "Companies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductActiveIngredient_ProductBasicDataId",
                table: "ProductActiveIngredient",
                column: "ProductBasicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CTDId",
                table: "Products",
                column: "CTDId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropTable(
                name: "ApprovedManufacturer");

            migrationBuilder.DropTable(
                name: "ApprovedPack");

            migrationBuilder.DropTable(
                name: "ApprovedPrice");

            migrationBuilder.DropTable(
                name: "ApprovedSupplier");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductActiveIngredient");

            migrationBuilder.DropTable(
                name: "ProductBasicData");
        }
    }
}
