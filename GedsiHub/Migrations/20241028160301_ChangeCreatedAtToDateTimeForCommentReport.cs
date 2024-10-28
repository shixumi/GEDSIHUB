using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    public partial class ChangeCreatedAtToDateTimeForCommentReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the TIMESTAMP column
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_comment_report_tbl");

            // Add the DATETIME2 column
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "forum_comment_report_tbl",
                type: "DATETIME2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6032));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6034));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6037));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6039));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6040));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 16, 3, 1, 4, DateTimeKind.Utc).AddTicks(6042));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the DATETIME2 column
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_comment_report_tbl");

            // Re-add the TIMESTAMP column
            migrationBuilder.AddColumn<byte[]>(
                name: "created_at",
                table: "forum_comment_report_tbl",
                type: "TIMESTAMP",
                nullable: false);
        }
    }
}
