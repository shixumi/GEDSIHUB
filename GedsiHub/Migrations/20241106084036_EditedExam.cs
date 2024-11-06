using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class EditedExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "FullMarks",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1356));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 8, 40, 34, 750, DateTimeKind.Utc).AddTicks(1382));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Duration",
                table: "Exams",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FullMarks",
                table: "Exams",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6642));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6644));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6648));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 5, 32, 20, 977, DateTimeKind.Utc).AddTicks(6653));
        }
    }
}
