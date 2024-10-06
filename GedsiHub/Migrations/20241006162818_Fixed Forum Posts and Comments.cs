using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    public partial class FixedForumPostsandComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the 'created_at' column which is of type 'TIMESTAMP'
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_comment_tbl");

            // Re-add the 'created_at' column as 'DATETIME'
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "forum_comment_tbl",
                type: "DATETIME",
                nullable: false,
                defaultValue: DateTime.UtcNow);  // Set the default to the current UTC time
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse migration: drop the 'created_at' column with 'DATETIME'
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_comment_tbl");

            // Re-add the 'created_at' column as 'TIMESTAMP'
            migrationBuilder.AddColumn<byte[]>(
                name: "created_at",
                table: "forum_comment_tbl",
                type: "TIMESTAMP",
                rowVersion: true,
                nullable: false);
        }
    }
}
