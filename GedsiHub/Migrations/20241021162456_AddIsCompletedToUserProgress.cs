using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedToUserProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7778));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7780));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 16, 13, 29, 811, DateTimeKind.Utc).AddTicks(7782));
        }
    }
}
