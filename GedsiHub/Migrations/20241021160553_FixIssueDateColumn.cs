using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class FixIssueDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_certificate_tbl_Modules_module_id",
                table: "certificate_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_certificate_tbl_user_tbl_user_id",
                table: "certificate_tbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_certificate_tbl",
                table: "certificate_tbl");

            migrationBuilder.RenameTable(
                name: "certificate_tbl",
                newName: "Certificates");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Certificates",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "module_id",
                table: "Certificates",
                newName: "ModuleId");

            migrationBuilder.RenameColumn(
                name: "certificate_id",
                table: "Certificates",
                newName: "CertificateId");

            migrationBuilder.RenameIndex(
                name: "IX_certificate_tbl_user_id",
                table: "Certificates",
                newName: "IX_Certificates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_certificate_tbl_module_id",
                table: "Certificates",
                newName: "IX_Certificates_ModuleId");

            // Drop the existing IssueDate column (which is a timestamp)
            migrationBuilder.DropColumn(
                name: "issue_date",
                table: "Certificates");

            // Add the new IssueDate column with DateTime type
            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Certificates",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            // Add new column CertificateUrl
            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates",
                column: "CertificateId");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 5, 52, 822, DateTimeKind.Utc).AddTicks(789));

            // Repeat for other ModuleId updates...

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Modules_ModuleId",
                table: "Certificates",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_user_tbl_UserId",
                table: "Certificates",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Modules_ModuleId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_user_tbl_UserId",
                table: "Certificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "Certificates");

            migrationBuilder.RenameTable(
                name: "Certificates",
                newName: "certificate_tbl");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "certificate_tbl",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "certificate_tbl",
                newName: "module_id");

            migrationBuilder.RenameColumn(
                name: "CertificateId",
                table: "certificate_tbl",
                newName: "certificate_id");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_UserId",
                table: "certificate_tbl",
                newName: "IX_certificate_tbl_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_ModuleId",
                table: "certificate_tbl",
                newName: "IX_certificate_tbl_module_id");

            // Add the old IssueDate column back as a timestamp
            migrationBuilder.AddColumn<byte[]>(
                name: "issue_date",
                table: "certificate_tbl",
                type: "TIMESTAMP",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_certificate_tbl",
                table: "certificate_tbl",
                column: "certificate_id");

            migrationBuilder.AddForeignKey(
                name: "FK_certificate_tbl_Modules_module_id",
                table: "certificate_tbl",
                column: "module_id",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_certificate_tbl_user_tbl_user_id",
                table: "certificate_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
