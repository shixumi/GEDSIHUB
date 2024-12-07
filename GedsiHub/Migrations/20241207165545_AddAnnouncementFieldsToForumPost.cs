using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncementFieldsToForumPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "forum_post_tbl",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnnouncement",
                table: "forum_post_tbl",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "forum_post_tbl",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5756));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5758));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5760));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5763));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5765));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 12, 7, 16, 55, 44, 829, DateTimeKind.Utc).AddTicks(5767));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "IsAnnouncement",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "forum_post_tbl");

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
    }
}
