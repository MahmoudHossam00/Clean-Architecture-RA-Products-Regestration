using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productlicenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MarketingTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumberYear = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastApprovedLayout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastApprovedLeaflet = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLicenses_LicenseCategories_LicenseCategoryId",
                        column: x => x.LicenseCategoryId,
                        principalTable: "LicenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductLicenses_MarketingTypes_MarketingTypeId",
                        column: x => x.MarketingTypeId,
                        principalTable: "MarketingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductLicenses_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCategories_Name",
                table: "LicenseCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketingTypes_Name",
                table: "MarketingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductLicenses_LicenseCategoryId",
                table: "ProductLicenses",
                column: "LicenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLicenses_MarketingTypeId",
                table: "ProductLicenses",
                column: "MarketingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLicenses_ProductId",
                table: "ProductLicenses",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLicenses");

            migrationBuilder.DropTable(
                name: "LicenseCategories");

            migrationBuilder.DropTable(
                name: "MarketingTypes");
        }
    }
}
