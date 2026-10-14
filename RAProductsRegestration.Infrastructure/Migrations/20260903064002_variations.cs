using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class variations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Variations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Initiator = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Cause = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    DeadlineNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approval = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariationRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    ResponsibleDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    VariationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariationRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariationRequirements_Departments_ResponsibleDepartmentId",
                        column: x => x.ResponsibleDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VariationRequirements_Variations_VariationId",
                        column: x => x.VariationId,
                        principalTable: "Variations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariationRequirements_ResponsibleDepartmentId",
                table: "VariationRequirements",
                column: "ResponsibleDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VariationRequirements_VariationId",
                table: "VariationRequirements",
                column: "VariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_ProductId",
                table: "Variations",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VariationRequirements");

            migrationBuilder.DropTable(
                name: "Variations");
        }
    }
}
