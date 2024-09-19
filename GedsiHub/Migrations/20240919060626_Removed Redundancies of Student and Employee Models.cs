using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRedundanciesofStudentandEmployeeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "differently_abled",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "gender_identity",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "indigenous_cultural_community",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "lived_name",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "middle_name",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "preferred_pronoun",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "sex",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "suffix",
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
                name: "sex",
                table: "employee_tbl");

            migrationBuilder.DropColumn(
                name: "suffix",
                table: "employee_tbl");

            migrationBuilder.AlterColumn<string>(
                name: "sector",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<string>(
                name: "position",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<string>(
                name: "branch_office_section_unit",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "student_tbl",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "differently_abled",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender_identity",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "indigenous_cultural_community",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lived_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "middle_name",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "student_tbl",
                type: "VARCHAR(15)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "preferred_pronoun",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sex",
                table: "student_tbl",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suffix",
                table: "student_tbl",
                type: "VARCHAR(10)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sector",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "position",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "branch_office_section_unit",
                table: "employee_tbl",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

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
                name: "sex",
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
    }
}
