using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPublishforLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Lessons");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5089));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5091));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5093));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5095));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5096));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 3, 12, 12, 38, 840, DateTimeKind.Utc).AddTicks(5098));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1752));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1755));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1757));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1759));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1761));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1763));
        }
    }
}
