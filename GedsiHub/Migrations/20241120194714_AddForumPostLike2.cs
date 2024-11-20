using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddForumPostLike2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "forum_post_like_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_post_like_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forum_post_like_tbl_forum_post_tbl_PostId",
                        column: x => x.PostId,
                        principalTable: "forum_post_tbl",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forum_post_like_tbl_user_tbl_UserId",
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
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1203));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1205));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1303));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 47, 13, 297, DateTimeKind.Utc).AddTicks(1304));

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_like_tbl_PostId",
                table: "forum_post_like_tbl",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_forum_post_like_tbl_UserId",
                table: "forum_post_like_tbl",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forum_post_like_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 19, 40, 1, 613, DateTimeKind.Utc).AddTicks(7139));
        }
    }
}
