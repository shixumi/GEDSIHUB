using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AnonymousFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make user_id nullable in forum_comment_tbl
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_tbl",
                type: "nvarchar(450)",
                nullable: true, // Allow null values
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Update the foreign key to use SET NULL on delete
            migrationBuilder.DropForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Update other data
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4938));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4940));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4967));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4969));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4970));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4972));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 8, 45, 894, DateTimeKind.Utc).AddTicks(4974));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert user_id to non-nullable
            migrationBuilder.DropForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_tbl",
                type: "nvarchar(450)",
                nullable: false, // Make it non-nullable again
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            // Restore the original foreign key behavior
            migrationBuilder.AddForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Revert other data updates
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6457));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6459));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6463));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6466));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 25, 16, 3, 29, 700, DateTimeKind.Utc).AddTicks(6468));
        }
    }
}
