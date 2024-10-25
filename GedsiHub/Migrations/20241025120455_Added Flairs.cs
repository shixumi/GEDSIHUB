using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddedFlairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "flair",
                table: "forum_post_tbl",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                table: "forum_post_tbl",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7969));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7972));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7974));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7976));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7978));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 12, 4, 54, 734, DateTimeKind.Utc).AddTicks(7979));

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_tbl_module_id",
                table: "forum_post_tbl",
                column: "module_id");

            migrationBuilder.AddForeignKey(
                name: "FK_forum_post_tbl_Modules_module_id",
                table: "forum_post_tbl",
                column: "module_id",
                principalTable: "Modules",
                principalColumn: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_forum_post_tbl_Modules_module_id",
                table: "forum_post_tbl");

            migrationBuilder.DropIndex(
                name: "IX_forum_post_tbl_module_id",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "flair",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "forum_post_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2198));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2200));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2201));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2203));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 17, 43, 18, 111, DateTimeKind.Utc).AddTicks(2208));
        }
    }
}
