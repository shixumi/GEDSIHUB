using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AnonymousForum : Migration
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
                name: "FK_forum_comment_report_tbl_user_tbl_user_id",
                table: "forum_comment_report_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_post_report_tbl_user_tbl_user_id",
                table: "forum_post_report_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_post_tbl_user_tbl_user_id",
                table: "forum_post_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl");

            // Make user_id columns nullable
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_report_tbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_tbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_post_report_tbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_post_tbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                name: "FK_forum_comment_report_tbl_user_tbl_user_id",
                table: "forum_comment_report_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_post_report_tbl_user_tbl_user_id",
                table: "forum_post_report_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_post_tbl_user_tbl_user_id",
                table: "forum_post_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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
                name: "FK_forum_comment_report_tbl_user_tbl_user_id",
                table: "forum_comment_report_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_post_report_tbl_user_tbl_user_id",
                table: "forum_post_report_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_forum_post_tbl_user_tbl_user_id",
                table: "forum_post_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_user_tbl_user_id",
                table: "student_tbl");

            // Revert user_id columns to non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_report_tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_comment_tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_post_report_tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "forum_post_tbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
                name: "FK_forum_comment_report_tbl_user_tbl_user_id",
                table: "forum_comment_report_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_comment_tbl_user_tbl_user_id",
                table: "forum_comment_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_post_report_tbl_user_tbl_user_id",
                table: "forum_post_report_tbl",
                column: "user_id",
                principalTable: "user_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_forum_post_tbl_user_tbl_user_id",
                table: "forum_post_tbl",
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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
