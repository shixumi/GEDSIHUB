using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GedsiHub.Migrations
{
    /// <inheritdoc />
    public partial class SeeddataforFAQsandModulesforChatbot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Answer", "Category", "Question" },
                values: new object[,]
                {
                    { 1, "GEDSI stands for Gender Equality, Diversity, and Social Inclusion. It focuses on promoting equality and inclusivity in various sectors.", "General", "What is GEDSI?" },
                    { 2, "To enroll in a course, simply navigate to the course catalog and click 'Enroll' on the course you're interested in.", "Courses", "How do I enroll in a course?" },
                    { 3, "You can reset your password by clicking on 'Forgot Password' on the login page and following the instructions.", "Account", "How do I reset my password?" },
                    { 4, "If you're having trouble logging in, please check your credentials or reset your password. If the issue persists, contact technical support.", "Technical Support", "Why can't I log in?" },
                    { 5, "Once you've completed all the required modules and assessments, your certificate will be automatically generated and available for download in your profile.", "Certificates", "How do I get a certificate after completing a course?" },
                    { 6, "You can view your course progress by going to the 'My Courses' section, where detailed analytics on your module completions will be displayed.", "Analytics", "How can I view my course progress?" },
                    { 7, "Your data is protected by our use of ASP.NET Core Data Protection, ensuring encryption at rest and in transit.", "Security", "How is my data protected?" },
                    { 8, "No, you need to complete all previous modules before accessing the final condensed learning module.", "Modules", "Can I access the final module directly?" },
                    { 9, "You can participate in forum discussions by navigating to the relevant course module and selecting the 'Forum' tab. Choose a topic and contribute to the discussion.", "Forum", "How can I participate in forum discussions?" },
                    { 10, "Yes, you can retake a quiz up to three times. After that, please contact your course administrator for further assistance.", "Courses", "Can I retake a quiz if I fail?" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "ModuleId", "CreatedDate", "Description", "LastModifiedDate", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(551), "This module covers the basics of gender equality, exploring the significance of gender equality in society and the workplace.", null, "Introduction to Gender Equality" },
                    { 2, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(553), "In this module, you'll learn about different gender identities, gender expression, and the importance of respecting personal pronouns.", null, "Understanding Gender Identities" },
                    { 3, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(555), "This module discusses how diversity and inclusion can benefit organizations and create a healthier work environment.", null, "Diversity and Inclusion in the Workplace" },
                    { 4, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(578), "Learn about how gender plays a role in global development, examining gender policies and frameworks used worldwide.", null, "Gender and Development: Global Perspectives" },
                    { 5, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(579), "This module introduces practical strategies for fostering social inclusion in various settings, from schools to workplaces.", null, "Social Inclusion Strategies" },
                    { 6, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(581), "This is the final module summarizing all previous modules, offering an interactive format to test your knowledge and understanding.", null, "Final Condensed Learning Module" },
                    { 7, new DateTime(2024, 10, 12, 9, 43, 45, 802, DateTimeKind.Utc).AddTicks(583), "This module educates about gender-based violence, its impact on individuals, and measures for prevention and support.", null, "Gender-Based Violence and Prevention" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 7);
        }
    }
}
