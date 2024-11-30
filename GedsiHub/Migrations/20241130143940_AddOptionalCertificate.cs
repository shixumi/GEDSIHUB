using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCertificateEnabled",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1750), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1752), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1755), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1757), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1759), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1761), true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                columns: new[] { "CreatedDate", "IsCertificateEnabled" },
                values: new object[] { new DateTime(2024, 11, 30, 14, 39, 39, 89, DateTimeKind.Utc).AddTicks(1763), true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCertificateEnabled",
                table: "Modules");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2967));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2969));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2971));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2973));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2976));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2977));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 26, 16, 10, 23, 498, DateTimeKind.Utc).AddTicks(2980));
        }
    }
}
