using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedLeaderboardModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "UserActivities");

            migrationBuilder.AddColumn<double>(
                name: "TimeSpentSeconds",
                table: "UserActivities",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5422));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5423));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5427));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5428));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5430));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 11, 11, 37, 121, DateTimeKind.Utc).AddTicks(5432));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpentSeconds",
                table: "UserActivities");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpent",
                table: "UserActivities",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6016));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6025));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 9, 28, 56, 906, DateTimeKind.Utc).AddTicks(6027));
        }
    }
}
