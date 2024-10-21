using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class RevampedAssessmentandModuleModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer_tbl");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropColumn(
                name: "ContentPath",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "H5PMetadata",
                table: "LessonContents");

            migrationBuilder.RenameColumn(
                name: "H5PId",
                table: "LessonContents",
                newName: "TextContent");

            migrationBuilder.AddColumn<int>(
                name: "PositionInt",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionInt",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ContentType",
                table: "LessonContents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "H5PEmbedCode",
                table: "LessonContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "LessonContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionInt",
                table: "LessonContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    AssessmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    H5PId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    H5PMetadata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.AssessmentId);
                    table.ForeignKey(
                        name: "FK_Assessments_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4302), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4304), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4306), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4307), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4309), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4311), 0 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                columns: new[] { "CreatedDate", "PositionInt" },
                values: new object[] { new DateTime(2024, 10, 21, 10, 9, 13, 233, DateTimeKind.Utc).AddTicks(4313), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                column: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropColumn(
                name: "PositionInt",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "PositionInt",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "H5PEmbedCode",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "PositionInt",
                table: "LessonContents");

            migrationBuilder.RenameColumn(
                name: "TextContent",
                table: "LessonContents",
                newName: "H5PId");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "LessonContents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ContentPath",
                table: "LessonContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "H5PMetadata",
                table: "LessonContents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId");
                });

            migrationBuilder.CreateTable(
                name: "answer_tbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_id = table.Column<int>(type: "int", nullable: false),
                    is_correct = table.Column<bool>(type: "bit", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_tbl", x => x.id);
                    table.ForeignKey(
                        name: "FK_answer_tbl_Questions_question_id",
                        column: x => x.question_id,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2729));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2731));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2734));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2736));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 15, 14, 46, 20, 803, DateTimeKind.Utc).AddTicks(2738));

            migrationBuilder.CreateIndex(
                name: "IX_answer_tbl_question_id",
                table: "answer_tbl",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ModuleId",
                table: "Questions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ModuleId",
                table: "Quizzes",
                column: "ModuleId");
        }
    }
}
