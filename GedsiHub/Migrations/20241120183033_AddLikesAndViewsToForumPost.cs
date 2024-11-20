using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndViewsToForumPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "likes_count",
                table: "forum_post_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "trending_score",
                table: "forum_post_tbl",
                type: "FLOAT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "views_count",
                table: "forum_post_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5237));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5238));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5240));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5242));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 18, 30, 32, 352, DateTimeKind.Utc).AddTicks(5244));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "likes_count",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "trending_score",
                table: "forum_post_tbl");

            migrationBuilder.DropColumn(
                name: "views_count",
                table: "forum_post_tbl");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6369));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 11, 20, 4, 30, 39, 905, DateTimeKind.Utc).AddTicks(6380));
        }
    }
}
