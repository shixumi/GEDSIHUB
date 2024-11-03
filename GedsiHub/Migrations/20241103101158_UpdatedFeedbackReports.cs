using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFeedbackReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ResultScore",
                table: "XApiStatements",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "XApiStatements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "XApiStatements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "UserActivities",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvidencePath",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2361));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2362));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2364));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2366));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2399));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 3, 10, 11, 57, 52, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.CreateIndex(
                name: "IX_XApiStatements_ModuleId",
                table: "XApiStatements",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_XApiStatements_Timestamp",
                table: "XApiStatements",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_XApiStatements_UserId",
                table: "XApiStatements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_XApiStatements_Modules_ModuleId",
                table: "XApiStatements",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_XApiStatements_user_tbl_UserId",
                table: "XApiStatements",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XApiStatements_Modules_ModuleId",
                table: "XApiStatements");

            migrationBuilder.DropForeignKey(
                name: "FK_XApiStatements_user_tbl_UserId",
                table: "XApiStatements");

            migrationBuilder.DropIndex(
                name: "IX_XApiStatements_ModuleId",
                table: "XApiStatements");

            migrationBuilder.DropIndex(
                name: "IX_XApiStatements_Timestamp",
                table: "XApiStatements");

            migrationBuilder.DropIndex(
                name: "IX_XApiStatements_UserId",
                table: "XApiStatements");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "XApiStatements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "XApiStatements");

            migrationBuilder.DropColumn(
                name: "EvidencePath",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<decimal>(
                name: "ResultScore",
                table: "XApiStatements",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "UserActivities",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8043));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8044));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8046));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8048));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8050));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8051));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 14, 23, 0, 114, DateTimeKind.Utc).AddTicks(8053));
        }
    }
}
