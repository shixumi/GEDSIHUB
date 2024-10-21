using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add 'is_completed' column to 'progress_module_tbl'
            migrationBuilder.AddColumn<bool>(
                name: "is_completed",
                table: "progress_module_tbl",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Update the Module creation dates
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3275));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3277));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3281));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 29, 12, 649, DateTimeKind.Utc).AddTicks(3286));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove 'is_completed' column from 'progress_module_tbl'
            migrationBuilder.DropColumn(
                name: "is_completed",
                table: "progress_module_tbl");

            // Revert the Module creation dates
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1203));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1205));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1206));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1210));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1233));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 24, 55, 509, DateTimeKind.Utc).AddTicks(1235));
        }
    }
}
