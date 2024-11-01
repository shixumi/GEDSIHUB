using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ImplementingxAPIwithAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_engagement_tbl");

            migrationBuilder.DropTable(
                name: "user_feedback_tbl");

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XApiStatements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorMbox = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorAccountHomePage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerbId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerbDisplay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectDefinitionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectDefinitionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResultSuccess = table.Column<bool>(type: "bit", nullable: true),
                    ContextRegistration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XApiStatements", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1135));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1137));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1141));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1142));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1144));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 1, 8, 38, 34, 101, DateTimeKind.Utc).AddTicks(1146));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "XApiStatements");

            migrationBuilder.CreateTable(
                name: "user_engagement_tbl",
                columns: table => new
                {
                    EngagementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsModuleCompleted = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuizScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_engagement_tbl", x => x.EngagementId);
                    table.ForeignKey(
                        name: "FK_user_engagement_tbl_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_engagement_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_feedback_tbl",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedbackDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SatisfactionScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_feedback_tbl", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_user_feedback_tbl_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_feedback_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6007));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 31, 4, 59, 6, 372, DateTimeKind.Utc).AddTicks(6011));

            migrationBuilder.CreateIndex(
                name: "IX_user_engagement_tbl_ModuleId",
                table: "user_engagement_tbl",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_engagement_tbl_UserId",
                table: "user_engagement_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_feedback_tbl_ModuleId",
                table: "user_feedback_tbl",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_feedback_tbl_UserId",
                table: "user_feedback_tbl",
                column: "UserId");
        }
    }
}
