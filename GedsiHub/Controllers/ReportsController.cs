using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GedsiHub.Models;
using GedsiHub.Repositories;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.IO;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportRepository reportRepository, ILogger<ReportsController> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Demographic()
        {
            var model = new DemographicReportViewModel
            {
                DateRangeOptions = GetDateRangeOptions(),
                CampusOptions = GetCampusOptions(),
                AgeGroupOptions = GetAgeGroupOptions(),
                SexOptions = GetSexOptions(),
                GenderIdentityOptions = GetGenderIdentityOptions(),
                UserTypeOptions = GetUserTypeOptions(),
                SortBy = "Name",
                GroupBy = "None",

                IncludeIdNumber = false,
                IncludeName = false,
                IncludeWebmail = false,
                IncludePhoneNumber = false,
                IncludeDateOfBirth = false,
                IncludeAge = false,
                IncludeSex = false,
                IncludeGender = false,
                IncludeIndigenousCommunity = false,
                IncludeDifferentlyAbled = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Demographic(DemographicReportViewModel model)
        {
            // Custom validation for required custom date fields
            if (!model.CustomStartDate.HasValue)
            {
                ModelState.AddModelError(nameof(model.CustomStartDate), "Please enter a start date.");
            }

            if (!model.CustomEndDate.HasValue)
            {
                ModelState.AddModelError(nameof(model.CustomEndDate), "Please enter an end date.");
            }

            // Validate that CustomStartDate is not later than CustomEndDate
            if (model.CustomStartDate.HasValue && model.CustomEndDate.HasValue)
            {
                if (model.CustomStartDate.Value > model.CustomEndDate.Value)
                {
                    ModelState.AddModelError(string.Empty, "The start date cannot be later than the end date.");
                }
                if (model.CustomEndDate.Value > DateTime.UtcNow)
                {
                    ModelState.AddModelError(nameof(model.CustomEndDate), "The end date cannot be in the future.");
                }
            }

            // If model state is invalid, log issues and return to view
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input and try again.";
                _logger.LogWarning("Model state is invalid.");

                // Log validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                }

                // Re-populate dropdown options for the view
                PopulateDropdownOptions(model);
                return View(model);
            }

            try
            {
                // Fetch users based on the provided filters
                _logger.LogInformation("Fetching users based on the provided filters.");
                var users = await _reportRepository.GetUsersAsync(model);

                // Check if any users were found
                if (users == null || !users.Any())
                {
                    _logger.LogInformation("No users found for the selected filters.");
                    TempData["NoDataMessage"] = "No users found for the selected filters.";
                    PopulateDropdownOptions(model);
                    return View(model);
                }

                // Log user details for debugging
                foreach (var user in users)
                {
                    int age = CalculateAge(user.DateOfBirth);
                    string ageGroup = DetermineAgeGroup(age);
                    _logger.LogInformation($"User: {user.FirstName} {user.LastName}, Age: {age}, Age Group: {ageGroup}");
                }

                // Log the report generation format
                _logger.LogInformation($"Generating report in {model.FileFormat} format.");

                // Generate report based on selected file format
                if (model.FileFormat == "CSV")
                {
                    var csvBytes = GenerateCsv(users, model);
                    _logger.LogInformation("CSV report generated successfully.");
                    return File(csvBytes, "text/csv", "DemographicReport.csv");
                }
                else if (model.FileFormat == "PDF")
                {
                    var pdfBytes = GeneratePdf(users, model);
                    _logger.LogInformation("PDF report generated successfully.");
                    return File(pdfBytes, "application/pdf", "DemographicReport.pdf");
                }

                // Handle invalid file format selection
                _logger.LogWarning("Invalid file format selected.");
                TempData["Error"] = "Please select a valid file format.";
                PopulateDropdownOptions(model);
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the error and return an error message
                _logger.LogError(ex, "An error occurred while generating the report.");
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                PopulateDropdownOptions(model);
                return View(model);
            }
        }

        // Helper method to populate dropdown options
        private void PopulateDropdownOptions(DemographicReportViewModel model)
        {
            model.CampusOptions = GetCampusOptions();
            model.AgeGroupOptions = GetAgeGroupOptions();
            model.SexOptions = GetSexOptions();
            model.GenderIdentityOptions = GetGenderIdentityOptions();
            model.UserTypeOptions = GetUserTypeOptions();
        }

        // Helper Methods for Options
        private List<SelectListItem> GetDateRangeOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "7days", Text = "Last 7 days" },
                new SelectListItem { Value = "28days", Text = "Last 28 days" },
                new SelectListItem { Value = "60days", Text = "Last 60 days" },
                new SelectListItem { Value = "custom", Text = "Custom" }
            };
        }

        private List<SelectListItem> GetCampusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "sta-mesa", Text = "Sta. Mesa, Manila (Main Campus)" }
            };
        }

        private List<SelectListItem> GetAgeGroupOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "15-19", Text = "15-19" },
                new SelectListItem { Value = "20-30", Text = "20-30" },
                new SelectListItem { Value = "31-40", Text = "31-40" },
                new SelectListItem { Value = "41-50", Text = "41-50" },
                new SelectListItem { Value = "51-60", Text = "51-60" },
                new SelectListItem { Value = "61+", Text = "61+" }
            };
        }

        private List<SelectListItem> GetSexOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "male", Text = "Male" },
                new SelectListItem { Value = "female", Text = "Female" }
            };
        }

        private List<SelectListItem> GetGenderIdentityOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Cisgender", Text = "Cisgender" },
                new SelectListItem { Value = "Transgender", Text = "Transgender" },
                new SelectListItem { Value = "Agender", Text = "Agender" },
                new SelectListItem { Value = "Gender Fluid", Text = "Gender Fluid" },
                new SelectListItem { Value = "Gender Queer", Text = "Gender Queer" }
            };
        }

        private List<SelectListItem> GetUserTypeOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "student", Text = "Student" },
                new SelectListItem { Value = "employee", Text = "Employee" }
            };
        }

        // Generate CSV Method
        private byte[] GenerateCsv(List<ApplicationUser> users, DemographicReportViewModel model)
        {
            var csvBuilder = new StringBuilder();

            // Add headers
            var headers = new List<string>();
            if (model.IncludeIdNumber) headers.Add("ID Number");
            if (model.IncludeName) headers.Add("Name");
            if (model.IncludeWebmail) headers.Add("Webmail");
            if (model.IncludePhoneNumber) headers.Add("Phone Number");
            if (model.IncludeDateOfBirth) headers.Add("Date of Birth");
            if (model.IncludeAge) headers.Add("Age");
            if (model.IncludeSex) headers.Add("Sex");
            if (model.IncludeGender) headers.Add("Gender Identity");
            if (model.IncludeIndigenousCommunity) headers.Add("Indigenous Community");
            if (model.IncludeDifferentlyAbled) headers.Add("Differently Abled");

            csvBuilder.AppendLine(string.Join(",", headers));

            // Add user data
            foreach (var user in users)
            {
                var row = new List<string>();
                if (model.IncludeIdNumber) row.Add($"\"{user.Id}\"");
                if (model.IncludeName) row.Add($"\"{user.FirstName} {user.LastName}\"");
                if (model.IncludeWebmail) row.Add($"\"{user.Email}\"");
                if (model.IncludePhoneNumber) row.Add($"\"{user.PhoneNumber}\"");
                if (model.IncludeDateOfBirth) row.Add($"\"{user.DateOfBirth:yyyy-MM-dd}\"");
                if (model.IncludeAge) row.Add($"\"{CalculateAge(user.DateOfBirth)}\"");
                if (model.IncludeSex) row.Add($"\"{user.Sex}\"");
                if (model.IncludeGender) row.Add($"\"{user.GenderIdentity}\"");
                if (model.IncludeIndigenousCommunity) row.Add(user.IsMemberOfIndigenousCommunity ? "Yes" : "No");
                if (model.IncludeDifferentlyAbled) row.Add(user.IsDisabled ? "Yes" : "No");

                csvBuilder.AppendLine(string.Join(",", row));
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        // Generate PDF Method
        private byte[] GeneratePdf(List<ApplicationUser> users, DemographicReportViewModel model)
        {
            // Configure QuestPDF license
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            using var ms = new MemoryStream();
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(210, 297, QuestPDF.Infrastructure.Unit.Millimetre);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(column =>
                    {
                        column.Item().Text("Demographic Report").FontSize(16).Bold();
                        column.Item().LineHorizontal(1);

                        // Create a table for user data
                        column.Item().Table(table =>
                        {
                            // Define table columns based on the Include flags
                            table.ColumnsDefinition(columns =>
                            {
                                if (model.IncludeIdNumber) columns.RelativeColumn();
                                if (model.IncludeName) columns.RelativeColumn();
                                if (model.IncludeWebmail) columns.RelativeColumn();
                                if (model.IncludePhoneNumber) columns.RelativeColumn();
                                if (model.IncludeDateOfBirth) columns.RelativeColumn();
                                if (model.IncludeAge) columns.RelativeColumn();
                                if (model.IncludeSex) columns.RelativeColumn();
                                if (model.IncludeGender) columns.RelativeColumn();
                            });

                            // Add table headers conditionally
                            table.Header(header =>
                            {
                                if (model.IncludeIdNumber) header.Cell().Element(CellStyle).Text("ID Number").Bold();
                                if (model.IncludeName) header.Cell().Element(CellStyle).Text("Name").Bold();
                                if (model.IncludeWebmail) header.Cell().Element(CellStyle).Text("Webmail").Bold();
                                if (model.IncludePhoneNumber) header.Cell().Element(CellStyle).Text("Phone Number").Bold();
                                if (model.IncludeDateOfBirth) header.Cell().Element(CellStyle).Text("Date of Birth").Bold();
                                if (model.IncludeAge) header.Cell().Element(CellStyle).Text("Age").Bold();
                                if (model.IncludeSex) header.Cell().Element(CellStyle).Text("Sex").Bold();
                                if (model.IncludeGender) header.Cell().Element(CellStyle).Text("Gender Identity").Bold();
                            });

                            // Add user data conditionally
                            foreach (var user in users)
                            {
                                if (model.IncludeIdNumber) table.Cell().Element(CellStyle).Text(user.Id);
                                if (model.IncludeName) table.Cell().Element(CellStyle).Text($"{user.FirstName} {user.LastName}");
                                if (model.IncludeWebmail) table.Cell().Element(CellStyle).Text(user.Email);
                                if (model.IncludePhoneNumber) table.Cell().Element(CellStyle).Text(user.PhoneNumber);
                                if (model.IncludeDateOfBirth) table.Cell().Element(CellStyle).Text(user.DateOfBirth.ToString("yyyy-MM-dd"));
                                if (model.IncludeAge) table.Cell().Element(CellStyle).Text(CalculateAge(user.DateOfBirth).ToString());
                                if (model.IncludeSex) table.Cell().Element(CellStyle).Text(user.Sex);
                                if (model.IncludeGender) table.Cell().Element(CellStyle).Text(user.GenderIdentity);
                            }
                        });
                    });
                });
            });

            document.GeneratePdf(ms);
            return ms.ToArray();

            // Helper method to style table cells
            static IContainer CellStyle(IContainer container) => container.Padding(5);
        }

        // Helper Method to Calculate Age
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        // Helper Method to Determine Age Group
        private string DetermineAgeGroup(int age)
        {
            if (age >= 15 && age <= 19) return "15-19";
            if (age >= 20 && age <= 30) return "20-30";
            if (age >= 31 && age <= 40) return "31-40";
            if (age >= 41 && age <= 50) return "41-50";
            if (age >= 51 && age <= 60) return "51-60";
            if (age >= 61) return "61+";
            return null;
        }
    }
}
