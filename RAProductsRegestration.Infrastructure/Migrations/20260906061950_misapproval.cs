using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAProductsRegestration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class misapproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MiscellaneousApprovalFileTypes",
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
                    table.PrimaryKey("PK_MiscellaneousApprovalFileTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiscellaneousApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Initiator = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ApprovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApprovalExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscellaneousApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscellaneousApprovals_MiscellaneousApprovalFileTypes_FileTypeId",
                        column: x => x.FileTypeId,
                        principalTable: "MiscellaneousApprovalFileTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiscellaneousApprovals_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalRequirement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    ResponsibleDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiscellaneousApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approval_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Approval_Attachment_AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approval_Attachment_FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approval_Attachment_Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementAttachment_AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequirementAttachment_FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequirementAttachment_Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalRequirement_MiscellaneousApprovals_MiscellaneousApprovalId",
                        column: x => x.MiscellaneousApprovalId,
                        principalTable: "MiscellaneousApprovals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequirement_MiscellaneousApprovalId",
                table: "ApprovalRequirement",
                column: "MiscellaneousApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_MiscellaneousApprovalFileTypes_Name",
                table: "MiscellaneousApprovalFileTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MiscellaneousApprovals_ApprovalExpiryDate",
                table: "MiscellaneousApprovals",
                column: "ApprovalExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_MiscellaneousApprovals_FileTypeId",
                table: "MiscellaneousApprovals",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MiscellaneousApprovals_ProductId",
                table: "MiscellaneousApprovals",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRequirement");

            migrationBuilder.DropTable(
                name: "MiscellaneousApprovals");

            migrationBuilder.DropTable(
                name: "MiscellaneousApprovalFileTypes");
        }
    }
}
