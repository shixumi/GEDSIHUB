using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class ForumTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing 'created_at' column
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "forum_post_tbl");

            // Add a new 'created_at' column with DATETIME2 type and a default value
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "forum_post_tbl",
                type: "DATETIME2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes by adding the 'timestamp' column again (if necessary)
            migrationBuilder.AddColumn<byte[]>(
                name: "created_at",
                table: "forum_post_tbl",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] { });
        }

    }
}
