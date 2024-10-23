using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModuletosupportPublishingandIconcolor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Modules",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6523), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6525), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6527), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6528), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6530), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6532), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                columns: new[] { "Color", "CreatedDate", "Status" },
                values: new object[] { "#000000", new DateTime(2024, 10, 23, 7, 13, 49, 126, DateTimeKind.Utc).AddTicks(6533), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                column: "ModuleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "H5PId",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8121));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8123));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8125));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8127));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8128));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8130));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 21, 18, 14, 44, 303, DateTimeKind.Utc).AddTicks(8132));

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                column: "ModuleId");
        }
    }
}
