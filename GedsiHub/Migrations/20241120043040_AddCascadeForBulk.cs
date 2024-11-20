using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeForBulk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_tbl_user_tbl_user_id",
                table: "admin_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_tbl_user_tbl_user_id",
                table: "employee_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6369));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6380));

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tbl_user_tbl_user_id",
                table: "admin_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_tbl_user_tbl_user_id",
                table: "employee_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_tbl_user_tbl_user_id",
                table: "admin_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_tbl_user_tbl_user_id",
                table: "employee_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5818));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 6, 17, 26, 43, 85, DateTimeKind.Utc).AddTicks(5822));

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tbl_user_tbl_user_id",
                table: "admin_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_tbl_user_tbl_user_id",
                table: "employee_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
