using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "college_dept_tbl",
                columns: table => new
                {
                    college_dept_id = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    college_name = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_college_dept_tbl", x => x.college_dept_id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "role_tbl",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_tbl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_tbl",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender_identity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    pronouns = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    is_indigenous_member = table.Column<bool>(type: "bit", nullable: false),
                    is_disabled = table.Column<bool>(type: "bit", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "DATE", nullable: false),
                    profile_picture_path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tbl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseContent",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContent", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_CourseContent_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
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
                name: "role_claim_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_claim_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_claim_tbl_role_tbl_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admin_tbl",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    admin_name = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_tbl", x => x.admin_id);
                    table.ForeignKey(
                        name: "FK_admin_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificate_tbl",
                columns: table => new
                {
                    certificate_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    issue_date = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_tbl", x => x.certificate_id);
                    table.ForeignKey(
                        name: "FK_certificate_tbl_Modules_module_id",
                        column: x => x.module_id,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_certificate_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee_tbl",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    emp_type = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    sector = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    emp_loc = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    position = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_tbl", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employee_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "enrollment_tbl",
                columns: table => new
                {
                    enrollment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    enrollment_date = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollment_tbl", x => x.enrollment_id);
                    table.ForeignKey(
                        name: "FK_enrollment_tbl_Modules_module_id",
                        column: x => x.module_id,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_enrollment_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "forum_post_tbl",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    tag = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_post_tbl", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_forum_post_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "progress_module_tbl",
                columns: table => new
                {
                    progress_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    progress_percentage = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    last_accessed = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progress_module_tbl", x => x.progress_id);
                    table.ForeignKey(
                        name: "FK_progress_module_tbl_Modules_module_id",
                        column: x => x.module_id,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progress_module_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_tbl",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    student_Lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    student_Fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    student_Mname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    college_dept_id = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    year_section = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_tbl", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_student_tbl_college_dept_tbl_college_dept_id",
                        column: x => x.college_dept_id,
                        principalTable: "college_dept_tbl",
                        principalColumn: "college_dept_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_claim_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_claim_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_claim_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login_tbl",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login_tbl", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_user_login_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role_tbl",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role_tbl", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_role_tbl_role_tbl_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token_tbl",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token_tbl", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_user_token_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: true)
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
                name: "forum_comment_tbl",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_comment_tbl", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_forum_comment_tbl_forum_post_tbl_post_id",
                        column: x => x.post_id,
                        principalTable: "forum_post_tbl",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forum_comment_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "answer_tbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_id = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    is_correct = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_admin_tbl_user_id",
                table: "admin_tbl",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_answer_tbl_question_id",
                table: "answer_tbl",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificate_tbl_module_id",
                table: "certificate_tbl",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificate_tbl_user_id",
                table: "certificate_tbl",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseContent_ModuleId",
                table: "CourseContent",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_employee_tbl_user_id",
                table: "employee_tbl",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_tbl_module_id",
                table: "enrollment_tbl",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_tbl_user_id",
                table: "enrollment_tbl",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_comment_tbl_post_id",
                table: "forum_comment_tbl",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_comment_tbl_user_id",
                table: "forum_comment_tbl",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_tbl_user_id",
                table: "forum_post_tbl",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_module_tbl_module_id",
                table: "progress_module_tbl",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_module_tbl_user_id",
                table: "progress_module_tbl",
                column: "user_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_role_claim_tbl_RoleId",
                table: "role_claim_tbl",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "role_tbl",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_student_tbl_college_dept_id",
                table: "student_tbl",
                column: "college_dept_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_tbl_user_id",
                table: "student_tbl",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_claim_tbl_UserId",
                table: "user_claim_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_tbl_UserId",
                table: "user_login_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_tbl_RoleId",
                table: "user_role_tbl",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "user_tbl",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_user_tbl_Email",
                table: "user_tbl",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_tbl_UserName",
                table: "user_tbl",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "user_tbl",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_tbl");

            migrationBuilder.DropTable(
                name: "answer_tbl");

            migrationBuilder.DropTable(
                name: "certificate_tbl");

            migrationBuilder.DropTable(
                name: "CourseContent");

            migrationBuilder.DropTable(
                name: "employee_tbl");

            migrationBuilder.DropTable(
                name: "enrollment_tbl");

            migrationBuilder.DropTable(
                name: "forum_comment_tbl");

            migrationBuilder.DropTable(
                name: "progress_module_tbl");

            migrationBuilder.DropTable(
                name: "role_claim_tbl");

            migrationBuilder.DropTable(
                name: "student_tbl");

            migrationBuilder.DropTable(
                name: "user_claim_tbl");

            migrationBuilder.DropTable(
                name: "user_login_tbl");

            migrationBuilder.DropTable(
                name: "user_role_tbl");

            migrationBuilder.DropTable(
                name: "user_token_tbl");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "forum_post_tbl");

            migrationBuilder.DropTable(
                name: "college_dept_tbl");

            migrationBuilder.DropTable(
                name: "role_tbl");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "user_tbl");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
