using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class FixedErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3640));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3651));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3653));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 19, 39, 49, 696, DateTimeKind.Utc).AddTicks(3654));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamID",
                table: "Questions",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionID",
                table: "Choices",
                column: "QuestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_QuestionID",
                table: "Choices",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "QuestionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamID",
                table: "Questions",
                column: "ExamID",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_QuestionID",
                table: "Choices");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamID",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ExamID",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Choices_QuestionID",
                table: "Choices");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2112));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2113));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2117));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 5, 18, 48, 27, 46, DateTimeKind.Utc).AddTicks(2124));
        }
    }
}
