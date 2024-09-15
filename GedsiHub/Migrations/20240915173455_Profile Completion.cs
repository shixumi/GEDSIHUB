using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ProfileCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "year_section",
                table: "student_tbl");

            migrationBuilder.RenameColumn(
                name: "student_Mname",
                table: "student_tbl",
                newName: "middle_name");

            migrationBuilder.RenameColumn(
                name: "student_Lname",
                table: "student_tbl",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "student_Fname",
                table: "student_tbl",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "emp_type",
                table: "employee_tbl",
                newName: "sex");

            migrationBuilder.RenameColumn(
                name: "emp_loc",
                table: "employee_tbl",
                newName: "branch_office_section_unit");

            migrationBuilder.AlterColumn<string>(
                name: "pronouns",
                table: "user_tbl",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_identity",
                table: "user_tbl",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "user_tbl",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "user_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "middle_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "student_tbl",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "college",
                table: "student_tbl",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "differently_abled",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "gender_identity",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "indigenous_cultural_community",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lived_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "student_tbl",
                type: "VARCHAR(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "preferred_pronoun",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "program",
                table: "student_tbl",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "section",
                table: "student_tbl",
                type: "VARCHAR(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sex",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "suffix",
                table: "student_tbl",
                type: "VARCHAR(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "student_tbl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "employee_tbl",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "differently_abled",
                table: "employee_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "employee_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "gender_identity",
                table: "employee_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "indigenous_cultural_community",
                table: "employee_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "employee_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lived_name",
                table: "employee_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "middle_name",
                table: "employee_tbl",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "employee_tbl",
                type: "VARCHAR(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "preferred_pronoun",
                table: "employee_tbl",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "suffix",
                table: "employee_tbl",
                type: "VARCHAR(10)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "user_tbl");

            migrationBuilder.DropColumn(
                name: "role",
                table: "user_tbl");

            migrationBuilder.DropColumn(
                name: "birthday",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "college",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "differently_abled",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "gender_identity",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "indigenous_cultural_community",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "lived_name",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "preferred_pronoun",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "program",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "section",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "sex",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "suffix",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "year",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "birthday",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "differently_abled",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "gender_identity",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "indigenous_cultural_community",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "lived_name",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "middle_name",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "preferred_pronoun",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "suffix",
                table: "employee_tbl");

            migrationBuilder.RenameColumn(
                name: "middle_name",
                table: "student_tbl",
                newName: "student_Mname");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "student_tbl",
                newName: "student_Lname");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "student_tbl",
                newName: "student_Fname");

            migrationBuilder.RenameColumn(
                name: "sex",
                table: "employee_tbl",
                newName: "emp_type");

            migrationBuilder.RenameColumn(
                name: "branch_office_section_unit",
                table: "employee_tbl",
                newName: "emp_loc");

            migrationBuilder.AlterColumn<string>(
                name: "pronouns",
                table: "user_tbl",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_identity",
                table: "user_tbl",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "student_Mname",
                table: "student_tbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)");

            migrationBuilder.AlterColumn<string>(
                name: "student_Lname",
                table: "student_tbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)");

            migrationBuilder.AlterColumn<string>(
                name: "student_Fname",
                table: "student_tbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)");

            migrationBuilder.AddColumn<string>(
                name: "year_section",
                table: "student_tbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
