using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ForumandReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "forum_post_tbl",
                type: "VARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "poll_options",
                table: "forum_post_tbl",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "forum_comment_tbl",
                type: "VARCHAR(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "forum_comment_report_tbl",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    reason = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_comment_report_tbl", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_forum_comment_report_tbl_forum_comment_tbl_comment_id",
                        column: x => x.comment_id,
                        principalTable: "forum_comment_tbl",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forum_comment_report_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "forum_post_report_tbl",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    reason = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<byte[]>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_post_report_tbl", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_forum_post_report_tbl_forum_post_tbl_post_id",
                        column: x => x.post_id,
                        principalTable: "forum_post_tbl",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forum_post_report_tbl_user_tbl_user_id",
                        column: x => x.user_id,
                        principalTable: "user_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_forum_comment_report_tbl_comment_id",
                table: "forum_comment_report_tbl",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_comment_report_tbl_user_id",
                table: "forum_comment_report_tbl",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_report_tbl_post_id",
                table: "forum_post_report_tbl",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_report_tbl_user_id",
                table: "forum_post_report_tbl",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forum_comment_report_tbl");

            migrationBuilder.DropTable(
                name: "forum_post_report_tbl");

            migrationBuilder.DropColumn(
                name: "image_path",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "poll_options",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "image_path",
                table: "forum_comment_tbl");
        }
    }
}
