using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ForumPostChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "forum_post_tbl",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "created_at",
                table: "forum_post_tbl",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }
    }
}
