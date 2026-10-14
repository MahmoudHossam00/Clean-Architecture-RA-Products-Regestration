using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class shelflifehistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelfLife",
                table: "ProductBasicData");

            migrationBuilder.CreateTable(
                name: "ProductBasicDataShelfLifeHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShelfLife = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBasicDataShelfLifeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBasicDataShelfLifeHistory_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasicDataShelfLifeHistory_ProductBasicDataId_VersionNumber",
                table: "ProductBasicDataShelfLifeHistory",
                columns: new[] { "ProductBasicDataId", "VersionNumber" },
                unique: true,
                filter: "[ProductBasicDataId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBasicDataShelfLifeHistory");

            migrationBuilder.AddColumn<string>(
                name: "ShelfLife",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
