using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class RemoveContentTypeFromLessonContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "LessonContents");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6007));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6011));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentType",
                table: "LessonContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7898));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7900));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7901));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7903));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7905));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7907));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 18, 30, 7, 182, DateTimeKind.Utc).AddTicks(7909));
        }
    }
}
