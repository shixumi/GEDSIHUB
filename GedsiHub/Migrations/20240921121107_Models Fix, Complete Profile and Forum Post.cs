using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ModelsFixCompleteProfileandForumPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_college_dept_tbl_college_dept_id",
                table: "student_tbl");

            migrationBuilder.DropTable(
                name: "college_dept_tbl");

            migrationBuilder.DropColumn(
                name: "college",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "program",
                table: "student_tbl");

            migrationBuilder.AlterColumn<int>(
                name: "college_dept_id",
                table: "student_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "course_id",
                table: "student_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CollegeDepartments",
                columns: table => new
                {
                    CollegeDeptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollegeDepartments", x => x.CollegeDeptId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CollegeDeptId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_CollegeDepartments_CollegeDeptId",
                        column: x => x.CollegeDeptId,
                        principalTable: "CollegeDepartments",
                        principalColumn: "CollegeDeptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CollegeDepartments",
                columns: new[] { "CollegeDeptId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "College of Accountancy and Finance (CAF)" },
                    { 2, "College of Architecture, Design and the Built Environment (CADBE)" },
                    { 3, "College of Arts and Letters (CAL)" },
                    { 4, "College of Business Administration (CBA)" },
                    { 5, "College of Communication (COC)" },
                    { 6, "College of Computer and Information Sciences (CCIS)" },
                    { 7, "College of Education (COED)" },
                    { 8, "College of Engineering (CE)" },
                    { 9, "College of Human Kinetics (CHK)" },
                    { 10, "College of Law (CL)" },
                    { 11, "College of Political Science and Public Administration (CPSPA)" },
                    { 12, "College of Social Sciences and Development (CSSD)" },
                    { 13, "College of Science (CS)" },
                    { 14, "College of Tourism, Hospitality and Transportation Management (CTHTM)" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CollegeDeptId", "CourseName" },
                values: new object[,]
                {
                    { 1, 1, "Bachelor of Science in Accountancy (BSA)" },
                    { 2, 1, "Bachelor of Science in Management Accounting (BSMA)" },
                    { 3, 1, "Bachelor of Science in Business Administration Major in Financial Management (BSBAFM)" },
                    { 4, 2, "Bachelor of Science in Architecture (BS-ARCH)" },
                    { 5, 2, "Bachelor of Science in Interior Design (BSID)" },
                    { 6, 2, "Bachelor of Science in Environmental Planning (BSEP)" },
                    { 7, 3, "Bachelor of Arts in English Language Studies (ABELS)" },
                    { 8, 3, "Bachelor of Arts in Filipinology (ABF)" },
                    { 9, 3, "Bachelor of Arts in Literary and Cultural Studies (ABLCS)" },
                    { 10, 3, "Bachelor of Arts in Philosophy (AB-PHILO)" },
                    { 11, 3, "Bachelor of Performing Arts major in Theater Arts (BPEA)" },
                    { 12, 4, "Doctor in Business Administration (DBA)" },
                    { 13, 4, "Master in Business Administration (MBA)" },
                    { 14, 4, "Bachelor of Science in Business Administration major in Human Resource Management (BSBAHRM)" },
                    { 15, 4, "Bachelor of Science in Business Administration major in Marketing Management (BSBA-MM)" },
                    { 16, 4, "Bachelor of Science in Entrepreneurship (BSENTREP)" },
                    { 17, 4, "Bachelor of Science in Office Administration (BSOA)" },
                    { 18, 5, "Bachelor in Advertising and Public Relations (BADPR)" },
                    { 19, 5, "Bachelor of Arts in Broadcasting (BA Broadcasting)" },
                    { 20, 5, "Bachelor of Arts in Communication Research (BACR)" },
                    { 21, 5, "Bachelor of Arts in Journalism (BAJ)" },
                    { 22, 6, "Bachelor of Science in Computer Science (BSCS)" },
                    { 23, 6, "Bachelor of Science in Information Technology (BSIT)" },
                    { 24, 7, "Doctor of Philsophy in Education Management (PhDEM)" },
                    { 25, 7, "Master of Arts in Education Management (MAEM) with Specialization in: Educational Leadership, Instructional Leadership" },
                    { 26, 7, "Master in Business Education (MBE)" },
                    { 27, 7, "Master in Library and Information Science (MLIS)" },
                    { 28, 7, "Master of Arts in English Language Teaching (MAELT)" },
                    { 29, 7, "Master of Arts in Education major in Mathematics Education (MAEd-ME)" },
                    { 30, 7, "Master of Arts in Physical Education and Sports (MAPES)" },
                    { 31, 7, "Master of Arts in Education major in Teaching in the Challenged Areas (MAED-TCA)" },
                    { 32, 7, "Post-Baccalaureate Diploma in Education (PBDE)" },
                    { 33, 7, "Bachelor of Technology and Livelihood Education (BTLEd) major in: Home Economics, Industrial Arts, Information and Communication Technology" },
                    { 34, 7, "Bachelor of Library and Information Science (BLIS)" },
                    { 35, 7, "Bachelor of Secondary Education (BSEd) major in: English, Mathematics, Science, Filipino, Social Studies" },
                    { 36, 7, "Bachelor of Elementary Education (BEEd)" },
                    { 37, 7, "Bachelor of Early Childhood Education (BECEd)" },
                    { 38, 8, "Bachelor of Science in Civil Engineering (BSCE)" },
                    { 39, 8, "Bachelor of Science in Computer Engineering (BSCpE)" },
                    { 40, 8, "Bachelor of Science in Electrical Engineering (BSEE)" },
                    { 41, 8, "Bachelor of Science in Electronics Engineering (BSECE)" },
                    { 42, 8, "Bachelor of Science in Industrial Engineering (BSIE)" },
                    { 43, 8, "Bachelor of Science in Mechanical Engineering (BSME)" },
                    { 44, 8, "Bachelor of Science in Railway Engineering (BSRE)" },
                    { 45, 9, "Bachelor of Physical Education (BPE)" },
                    { 46, 9, "Bachelor of Science in Exercises and Sports (BSESS)" },
                    { 47, 10, "Juris Doctor (JD)" },
                    { 48, 11, "Doctor in Public Administration (DPA)" },
                    { 49, 11, "Master in Public Administration (MPA)" },
                    { 50, 11, "Bachelor of Public Administration (BPA)" },
                    { 51, 11, "Bachelor of Arts in International Studies (BAIS)" },
                    { 52, 11, "Bachelor of Arts in Political Economy (BAPE)" },
                    { 53, 11, "Bachelor of Arts in Political Science (BAPS)" },
                    { 54, 12, "Bachelor of Arts in History (BAH)" },
                    { 55, 12, "Bachelor of Arts in Sociology (BAS)" },
                    { 56, 12, "Bachelor of Science in Cooperatives (BSC)" },
                    { 57, 12, "Bachelor of Science in Economics (BSE)" },
                    { 58, 12, "Bachelor of Science in Psychology (BSPSY)" },
                    { 59, 13, "Bachelor of Science Food Technology (BSFT)" },
                    { 60, 13, "Bachelor of Science in Applied Mathematics (BSAPMATH)" },
                    { 61, 13, "Bachelor of Science in Biology (BSBIO)" },
                    { 62, 13, "Bachelor of Science in Chemistry (BSCHEM)" },
                    { 63, 13, "Bachelor of Science in Mathematics (BSMATH)" },
                    { 64, 13, "Bachelor of Science in Nutrition and Dietetics (BSND)" },
                    { 65, 13, "Bachelor of Science in Physics (BSPHY)" },
                    { 66, 13, "Bachelor of Science in Statistics (BSSTAT)" },
                    { 67, 14, "Bachelor of Science in Hospitality Management (BSHM)" },
                    { 68, 14, "Bachelor of Science in Tourism Management (BSTM)" },
                    { 69, 14, "Bachelor of Science in Transportation Management (BSTRM)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_tbl_course_id",
                table: "student_tbl",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CollegeDeptId",
                table: "Courses",
                column: "CollegeDeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_CollegeDepartments_college_dept_id",
                table: "student_tbl",
                column: "college_dept_id",
                principalTable: "CollegeDepartments",
                principalColumn: "CollegeDeptId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl",
                column: "course_id",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_CollegeDepartments_college_dept_id",
                table: "student_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tbl_Courses_course_id",
                table: "student_tbl");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CollegeDepartments");

            migrationBuilder.DropIndex(
                name: "IX_student_tbl_course_id",
                table: "student_tbl");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "student_tbl");

            migrationBuilder.AlterColumn<string>(
                name: "college_dept_id",
                table: "student_tbl",
                type: "VARCHAR(30)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "college",
                table: "student_tbl",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "program",
                table: "student_tbl",
                type: "VARCHAR(100)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_student_tbl_college_dept_tbl_college_dept_id",
                table: "student_tbl",
                column: "college_dept_id",
                principalTable: "college_dept_tbl",
                principalColumn: "college_dept_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
