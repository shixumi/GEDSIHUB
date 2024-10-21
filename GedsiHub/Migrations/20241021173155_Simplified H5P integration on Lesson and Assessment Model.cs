using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedH5PintegrationonLessonandAssessmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "H5PId",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "H5PMetadata",
                table: "Assessments",
                newName: "H5PEmbedCode");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5727));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5729));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5731));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5733));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5735));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5738));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 17, 31, 54, 357, DateTimeKind.Utc).AddTicks(5739));

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                column: "ModuleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "H5PEmbedCode",
                table: "Assessments",
                newName: "H5PMetadata");

            migrationBuilder.AddColumn<string>(
                name: "H5PId",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4302));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4304));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4306));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4307));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4309));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4311));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4313));

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                column: "ModuleId");
        }
    }
}
