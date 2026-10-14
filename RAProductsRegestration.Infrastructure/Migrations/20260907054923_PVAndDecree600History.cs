using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PVAndDecree600History : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decree600_Deadline",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "Decree600_Description",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "Decree600_FulfillmentDate",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "Decree600_IsFulfilled",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "Decree600_Notes",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_Deadline",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_Description",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_FulfillmentDate",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_IsFulfilled",
                table: "ProductBasicData");

            migrationBuilder.DropColumn(
                name: "PV_Notes",
                table: "ProductBasicData");

            migrationBuilder.CreateTable(
                name: "Decree600DeadlineHistoryItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deadline_Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    Deadline_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline_FulfillmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Deadline_IsFulfilled = table.Column<bool>(type: "bit", nullable: false),
                    Deadline_Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decree600DeadlineHistoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decree600DeadlineHistoryItem_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PVDeadlineHistoryItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    ProductBasicDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deadline_Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    Deadline_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline_FulfillmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Deadline_IsFulfilled = table.Column<bool>(type: "bit", nullable: false),
                    Deadline_Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PVDeadlineHistoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PVDeadlineHistoryItem_ProductBasicData_ProductBasicDataId",
                        column: x => x.ProductBasicDataId,
                        principalTable: "ProductBasicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decree600DeadlineHistoryItem_ProductBasicDataId",
                table: "Decree600DeadlineHistoryItem",
                column: "ProductBasicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PVDeadlineHistoryItem_ProductBasicDataId",
                table: "PVDeadlineHistoryItem",
                column: "ProductBasicDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Decree600DeadlineHistoryItem");

            migrationBuilder.DropTable(
                name: "PVDeadlineHistoryItem");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Decree600_Deadline",
                table: "ProductBasicData",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Decree600_Description",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Decree600_FulfillmentDate",
                table: "ProductBasicData",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Decree600_IsFulfilled",
                table: "ProductBasicData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Decree600_Notes",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PV_Deadline",
                table: "ProductBasicData",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PV_Description",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PV_FulfillmentDate",
                table: "ProductBasicData",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PV_IsFulfilled",
                table: "ProductBasicData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PV_Notes",
                table: "ProductBasicData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
