using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class Addedmodelsforanalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "active_user_tbl",
                columns: table => new
                {
                    ActiveUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_active_user_tbl", x => x.ActiveUserId);
                    table.ForeignKey(
                        name: "FK_active_user_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "module_activity_tbl",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "time", nullable: false),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_activity_tbl", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_module_activity_tbl_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_module_activity_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_engagement_tbl",
                columns: table => new
                {
                    EngagementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    QuizScore = table.Column<int>(type: "int", nullable: false),
                    IsModuleCompleted = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SatisfactionScore = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedbackDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "user_login_analytics_tbl",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login_analytics_tbl", x => x.LoginId);
                    table.ForeignKey(
                        name: "FK_user_login_analytics_tbl_user_tbl_UserId",
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
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3683));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3688));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3690));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 23, 43, 579, DateTimeKind.Utc).AddTicks(3694));

            migrationBuilder.CreateIndex(
                name: "IX_active_user_tbl_UserId",
                table: "active_user_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_module_activity_tbl_ModuleId",
                table: "module_activity_tbl",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_module_activity_tbl_UserId",
                table: "module_activity_tbl",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_user_login_analytics_tbl_UserId",
                table: "user_login_analytics_tbl",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "active_user_tbl");

            migrationBuilder.DropTable(
                name: "module_activity_tbl");

            migrationBuilder.DropTable(
                name: "user_engagement_tbl");

            migrationBuilder.DropTable(
                name: "user_feedback_tbl");

            migrationBuilder.DropTable(
                name: "user_login_analytics_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7923));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7924));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7926));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7928));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7929));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 11, 38, 47, 316, DateTimeKind.Utc).AddTicks(7931));
        }
    }
}
