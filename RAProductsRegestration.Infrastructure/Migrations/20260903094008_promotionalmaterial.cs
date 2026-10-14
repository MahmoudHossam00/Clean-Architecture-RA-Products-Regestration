using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class promotionalmaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromotionalMaterialTypes",
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
                    table.PrimaryKey("PK_PromotionalMaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionalMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionalMaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialFileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaterialFileNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApprovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApprovalAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApprovalFileNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApprovalExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionalMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionalMaterials_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionalMaterials_PromotionalMaterialTypes_PromotionalMaterialTypeId",
                        column: x => x.PromotionalMaterialTypeId,
                        principalTable: "PromotionalMaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionalMaterials_ApprovalExpiryDate",
                table: "PromotionalMaterials",
                column: "ApprovalExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionalMaterials_ProductId",
                table: "PromotionalMaterials",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionalMaterials_PromotionalMaterialTypeId",
                table: "PromotionalMaterials",
                column: "PromotionalMaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionalMaterialTypes_Name",
                table: "PromotionalMaterialTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionalMaterials");

            migrationBuilder.DropTable(
                name: "PromotionalMaterialTypes");
        }
    }
}
