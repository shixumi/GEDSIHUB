using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    public partial class ChangeCreatedAtToDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing 'created_at' column (timestamp)
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_post_report_tbl");

            // Add a new 'created_at' column with DATETIME2
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "forum_post_report_tbl",
                type: "DATETIME2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            // Existing code for updating 'Modules' table data (if necessary)
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9835));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 35, 59, 634, DateTimeKind.Utc).AddTicks(9840));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert: Drop the 'created_at' column and add the old timestamp column
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_post_report_tbl");

            migrationBuilder.AddColumn<byte[]>(
                name: "created_at",
                table: "forum_post_report_tbl",
                type: "TIMESTAMP",
                nullable: false,
                defaultValue: new byte[] { });

            // Revert the data changes (if necessary)
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4016));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4019));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4021));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4023));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4025));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 15, 28, 5, 720, DateTimeKind.Utc).AddTicks(4029));
        }
    }
}
