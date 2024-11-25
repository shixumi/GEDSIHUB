using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AnonymousFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateModuleCreatedDates(migrationBuilder, new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            UpdateModuleCreatedDates(migrationBuilder, new DateTime(2024, 11, 25, 15, 48, 23, 946, DateTimeKind.Utc));
        }

        /// <summary>
        /// Updates the CreatedDate for all modules with the provided date.
        /// </summary>
        /// <param name="migrationBuilder">The migration builder.</param>
        /// <param name="createdDate">The DateTime value to apply to the CreatedDate column.</param>
        private void UpdateModuleCreatedDates(MigrationBuilder migrationBuilder, DateTime createdDate)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: createdDate.AddTicks(6457));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: createdDate.AddTicks(6459));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: createdDate.AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: createdDate.AddTicks(6463));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: createdDate.AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: createdDate.AddTicks(6466));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: createdDate.AddTicks(6468));
        }
    }
}
