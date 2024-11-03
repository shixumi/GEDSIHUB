using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatdUserProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_accessed",
                table: "progress_module_tbl");

            migrationBuilder.AddColumn<string>(
                name: "completed_lesson_ids",
                table: "progress_module_tbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5927));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5932));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5934));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5937));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5939));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 18, 26, 32, 657, DateTimeKind.Utc).AddTicks(5941));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "completed_lesson_ids",
                table: "progress_module_tbl");

            migrationBuilder.AddColumn<byte[]>(
                name: "last_accessed",
                table: "progress_module_tbl",
                type: "TIMESTAMP",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6772));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6777));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 33, 26, 809, DateTimeKind.Utc).AddTicks(6781));
        }
    }
}
