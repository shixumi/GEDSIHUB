using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ChatbotCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 13, 6, 17, 954, DateTimeKind.Utc).AddTicks(7460));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3544));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3546));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3548));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3549));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3551));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3553));
        }
    }
}
