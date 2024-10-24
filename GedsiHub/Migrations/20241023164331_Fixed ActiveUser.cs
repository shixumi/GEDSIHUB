using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class FixedActiveUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_active_user_tbl_user_tbl_UserId",
                table: "active_user_tbl");

            migrationBuilder.RenameColumn(
                name: "ActiveUserId",
                table: "active_user_tbl",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "active_user_tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7718));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7720));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7722));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7723));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7734));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 16, 43, 30, 319, DateTimeKind.Utc).AddTicks(7736));

            migrationBuilder.CreateIndex(
                name: "IX_active_user_tbl_ConnectionId",
                table: "active_user_tbl",
                column: "ConnectionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_active_user_tbl_ConnectionId",
                table: "active_user_tbl");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "active_user_tbl");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "active_user_tbl",
                newName: "ActiveUserId");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3683));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3688));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3690));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3694));

            migrationBuilder.AddForeignKey(
                name: "FK_active_user_tbl_user_tbl_UserId",
                table: "active_user_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
