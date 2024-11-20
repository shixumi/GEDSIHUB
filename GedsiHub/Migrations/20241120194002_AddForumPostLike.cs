using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddForumPostLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7139));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5237));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5238));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5240));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5242));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5244));
        }
    }
}
