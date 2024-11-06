using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorOnAnswerRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Choices_ChoiceID",
                table: "Answers");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1422));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1454));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1457));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1459));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1461));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 4, 38, 523, DateTimeKind.Utc).AddTicks(1462));

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Choices_ChoiceID",
                table: "Answers",
                column: "ChoiceID",
                principalTable: "Choices",
                principalColumn: "ChoiceID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Choices_ChoiceID",
                table: "Answers");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1805));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1807));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1809));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1810));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1812));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 7, 42, 16, 543, DateTimeKind.Utc).AddTicks(1815));

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Choices_ChoiceID",
                table: "Answers",
                column: "ChoiceID",
                principalTable: "Choices",
                principalColumn: "ChoiceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
