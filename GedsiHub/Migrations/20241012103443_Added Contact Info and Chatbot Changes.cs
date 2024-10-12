using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class AddedContactInfoandChatbotChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TikTok = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    X = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContactInfos",
                columns: new[] { "Id", "Facebook", "Instagram", "PhoneNumber", "SupportEmail", "TikTok", "Website", "X" },
                values: new object[] { 1, "https://www.facebook.com/gadpup", "https://www.instagram.com/pupgadofficial", "+1-800-123-4567", "dev.gedsihub@gmail.com", "https://www.tiktok.com/@yourprofile", "https://www.pup.edu.ph/research/gado/", "https://x.com/PUPGADO" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3544));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3546));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3548));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3549));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3551));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 10, 34, 42, 642, DateTimeKind.Utc).AddTicks(3553));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInfos");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(551));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(553));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(555));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(578));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(581));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(583));
        }
    }
}
