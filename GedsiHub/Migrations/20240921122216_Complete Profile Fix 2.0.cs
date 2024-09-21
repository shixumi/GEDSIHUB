using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class CompleteProfileFix20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl");

            migrationBuilder.AlterColumn<int>(
                name: "course_id",
                table: "student_tbl",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl",
                column: "course_id",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl");

            migrationBuilder.AlterColumn<int>(
                name: "course_id",
                table: "student_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl",
                column: "course_id",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
