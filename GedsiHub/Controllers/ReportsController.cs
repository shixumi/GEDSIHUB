// GedsiHub.Controllers.ReportsController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Repositories;
using GedsiHub.Services;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly AnalyticsService _analyticsService;
        private readonly IReportGenerationService _reportGenerationService;

        public ReportsController(
            IReportRepository reportRepository,
            ILogger<ReportsController> logger,
            ApplicationDbContext context,
            AnalyticsService analyticsService,
            IReportGenerationService reportGenerationService) // Inject the service
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _context = context;
            _analyticsService = analyticsService;
            _reportGenerationService = reportGenerationService;
        }

        // GET: Reports/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new CombinedReportViewModel
            {
                DemographicReport = new DemographicReportViewModel
                {
                    CampusOptions = GetCampusOptions(),
                    AgeGroupOptions = GetAgeGroupOptions(),
                    SexOptions = GetSexOptions(),
                    GenderIdentityOptions = GetGenderIdentityOptions(),
                    UserTypeOptions = GetUserTypeOptions(),
                    GroupByOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Select Group By", Disabled = true, Selected = true },
                        new SelectListItem { Value = "none", Text = "None" },
                        new SelectListItem { Value = "age", Text = "Age" },
                        new SelectListItem { Value = "sex", Text = "Sex" },
                        new SelectListItem { Value = "gender", Text = "Gender Identity" }
                    },
                    SortByOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Select Sort By", Disabled = true, Selected = true },
                        new SelectListItem { Value = "name", Text = "Name" },
                        new SelectListItem { Value = "age", Text = "Age" },
                        new SelectListItem { Value = "date", Text = "Date" }
                    }
                },
                ModuleReport = new ModuleReportViewModel
                {
                    ModuleOptions = await GetModuleOptionsAsync()
                }
            };

            return View(model);
        }

        // POST: Reports/Demographic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Demographic(DemographicReportViewModel model)
        {
            // Validate that at least one metric is selected
            if (!model.IncludeIdNumber && !model.IncludeName && !model.IncludeWebmail &&
                !model.IncludePhoneNumber && !model.IncludeDateOfBirth && !model.IncludeAge &&
                !model.IncludeSex && !model.IncludeGender && !model.IncludeIndigenousCommunity &&
                !model.IncludeDifferentlyAbled)
            {
                ModelState.AddModelError("", "Please select at least one metric.");
            }

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
                model.CampusOptions = GetCampusOptions();
                model.AgeGroupOptions = GetAgeGroupOptions();
                model.SexOptions = GetSexOptions();
                model.GenderIdentityOptions = GetGenderIdentityOptions();
                model.UserTypeOptions = GetUserTypeOptions();

                var combinedModel = new CombinedReportViewModel
                {
                    DemographicReport = model,
                    ModuleReport = new ModuleReportViewModel
                    {
                        ModuleOptions = await GetModuleOptionsAsync()
                    }
                };

                return View("Index", combinedModel);
            }

            try
            {
                // **Logging Each Filter Value**
                _logger.LogInformation("Demographic Report Generation Initiated with the following filters:");
                _logger.LogInformation($"CustomStartDate: {model.CustomStartDate?.ToString("yyyy-MM-dd") ?? "Not Provided"}");
                _logger.LogInformation($"CustomEndDate: {model.CustomEndDate?.ToString("yyyy-MM-dd") ?? "Not Provided"}");
                _logger.LogInformation($"Campus: {model.Campus}");
                _logger.LogInformation($"AgeGroup: {model.AgeGroup}");
                _logger.LogInformation($"Sex: {model.Sex}");
                _logger.LogInformation($"GenderIdentity: {model.GenderIdentity}");
                _logger.LogInformation($"UserType: {model.UserType}");
                _logger.LogInformation($"GroupBy: {model.GroupBy}");
                _logger.LogInformation($"SortBy: {model.SortBy}");
                _logger.LogInformation($"FileFormat: {model.FileFormat}");

                // **Logging Selected Metrics**
                _logger.LogInformation("Selected Metrics to Include:");
                _logger.LogInformation($"IncludeIdNumber: {model.IncludeIdNumber}");
                _logger.LogInformation($"IncludeName: {model.IncludeName}");
                _logger.LogInformation($"IncludeWebmail: {model.IncludeWebmail}");
                _logger.LogInformation($"IncludePhoneNumber: {model.IncludePhoneNumber}");
                _logger.LogInformation($"IncludeDateOfBirth: {model.IncludeDateOfBirth}");
                _logger.LogInformation($"IncludeAge: {model.IncludeAge}");
                _logger.LogInformation($"IncludeSex: {model.IncludeSex}");
                _logger.LogInformation($"IncludeGender: {model.IncludeGender}");
                _logger.LogInformation($"IncludeIndigenousCommunity: {model.IncludeIndigenousCommunity}");
                _logger.LogInformation($"IncludeDifferentlyAbled: {model.IncludeDifferentlyAbled}");

                // **Fetching Users Based on Filters**
                _logger.LogInformation("Fetching users based on the provided filters.");
                var users = await _reportRepository.GetUsersAsync(model);

                // Check if any users were found
                if (users == null || !users.Any())
                {
                    _logger.LogInformation("No users found for the selected filters.");
                    TempData["NoDataMessage"] = "No users found for the selected filters.";

                    // Re-populate dropdown options for the view
                    model.CampusOptions = GetCampusOptions();
                    model.AgeGroupOptions = GetAgeGroupOptions();
                    model.SexOptions = GetSexOptions();
                    model.GenderIdentityOptions = GetGenderIdentityOptions();
                    model.UserTypeOptions = GetUserTypeOptions();

                    var combinedModel = new CombinedReportViewModel
                    {
                        DemographicReport = model,
                        ModuleReport = new ModuleReportViewModel
                        {
                            ModuleOptions = await GetModuleOptionsAsync()
                        }
                    };

                    return View("Index", combinedModel);
                }

                // **Logging User Details for Debugging**
                foreach (var user in users)
                {
                    int age = CalculateAge(user.DateOfBirth);
                    string ageGroup = DetermineAgeGroup(age);
                    _logger.LogInformation($"User: {user.FirstName} {user.LastName}, Age: {age}, Age Group: {ageGroup}");
                }

                // **Log the report generation format**
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

                // Re-populate dropdown options for the view
                model.CampusOptions = GetCampusOptions();
                model.AgeGroupOptions = GetAgeGroupOptions();
                model.SexOptions = GetSexOptions();
                model.GenderIdentityOptions = GetGenderIdentityOptions();
                model.UserTypeOptions = GetUserTypeOptions();

                var combinedModelInvalidFormat = new CombinedReportViewModel
                {
                    DemographicReport = model,
                    ModuleReport = new ModuleReportViewModel
                    {
                        ModuleOptions = await GetModuleOptionsAsync()
                    }
                };

                return View("Index", combinedModelInvalidFormat);
            }
            catch (Exception ex)
            {
                // Log the error and return an error message
                _logger.LogError(ex, "An error occurred while generating the report.");
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;

                // Re-populate dropdown options for the view
                model.CampusOptions = GetCampusOptions();
                model.AgeGroupOptions = GetAgeGroupOptions();
                model.SexOptions = GetSexOptions();
                model.GenderIdentityOptions = GetGenderIdentityOptions();
                model.UserTypeOptions = GetUserTypeOptions();

                var combinedModelException = new CombinedReportViewModel
                {
                    DemographicReport = model,
                    ModuleReport = new ModuleReportViewModel
                    {
                        ModuleOptions = await GetModuleOptionsAsync()
                    }
                };
                return View("Index", combinedModelException);
            }
        }

        // POST: Reports/ModuleReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModuleReport(ModuleReportViewModel model)
        {
            _logger.LogInformation("Module Report Generation Initiated.");
            _logger.LogDebug($"ModuleOptions is {(model.ModuleOptions == null ? "null" : "not null")} on POST.");
            ModelState.Remove(nameof(ModuleReportViewModel.ModuleOptions));

            // Validate that at least one metric is selected
            if (!model.IncludeCompletionRate && !model.IncludeAverageQuizScore && !model.IncludeCertificatesIssued)
            {
                ModelState.AddModelError("", "Please select at least one metric to include in the report.");
                _logger.LogWarning("No metrics selected for Module Report.");
            }

            // Validate file format
            if (string.IsNullOrEmpty(model.FileFormat))
            {
                ModelState.AddModelError(nameof(model.FileFormat), "Please select a file format.");
                _logger.LogWarning("No file format selected for Module Report.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input and try again.";
                _logger.LogWarning("Model state is invalid for Module Report.");

                // Log validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                }

                // Re-populate ModuleOptions for the view
                model.ModuleOptions = await GetModuleOptionsAsync();

                var combinedModel = new CombinedReportViewModel
                {
                    DemographicReport = new DemographicReportViewModel
                    {
                        CampusOptions = GetCampusOptions(),
                        AgeGroupOptions = GetAgeGroupOptions(),
                        SexOptions = GetSexOptions(),
                        GenderIdentityOptions = GetGenderIdentityOptions(),
                        UserTypeOptions = GetUserTypeOptions(),
                    },
                    ModuleReport = model
                };
                return View("Index", combinedModel);
            }

            try
            {
                _logger.LogInformation($"Selected Module ID: {model.SelectedModuleId}");

                var module = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == model.SelectedModuleId);
                if (module == null)
                {
                    TempData["ErrorMessage"] = "Selected module not found.";
                    _logger.LogWarning($"Module with ID {model.SelectedModuleId} not found.");
                    return RedirectToAction("Index");
                }

                byte[] reportBytes;
                string mimeType;
                string fileName;

                var userName = User.Identity.Name; // Assuming user is authenticated
                _logger.LogInformation($"Generating Module Report for Module: {module.Title}, User: {userName}, Format: {model.FileFormat}");

                if (model.FileFormat == "CSV")
                {
                    reportBytes = await _reportGenerationService.GenerateModuleCsvAsync(module, model, userName);
                    mimeType = "text/csv";
                    fileName = "ModuleReport.csv";
                    _logger.LogInformation("Module CSV report generated successfully.");
                }
                else if (model.FileFormat == "PDF")
                {
                    reportBytes = await _reportGenerationService.GenerateModulePdfAsync(module, model, userName);
                    mimeType = "application/pdf";
                    fileName = "ModuleReport.pdf";
                    _logger.LogInformation("Module PDF report generated successfully.");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid file format selected.";
                    _logger.LogWarning($"Invalid file format selected: {model.FileFormat}");

                    // Re-populate dropdown options for the view
                    model.ModuleOptions = await GetModuleOptionsAsync();

                    var combinedModelInvalidFormat = new CombinedReportViewModel
                    {
                        DemographicReport = new DemographicReportViewModel
                        {
                            CampusOptions = GetCampusOptions(),
                            AgeGroupOptions = GetAgeGroupOptions(),
                            SexOptions = GetSexOptions(),
                            GenderIdentityOptions = GetGenderIdentityOptions(),
                            UserTypeOptions = GetUserTypeOptions(),
                        },
                        ModuleReport = model
                    };
                    return View("Index", combinedModelInvalidFormat);
                }

                _logger.LogInformation($"Module report generated successfully for Module ID: {module.ModuleId}, Format: {model.FileFormat}");
                return File(reportBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the module report.");
                TempData["ErrorMessage"] = "An error occurred while generating the report. Please try again later.";

                // Re-populate dropdown options for the view
                model.ModuleOptions = await GetModuleOptionsAsync();

                var combinedModelException = new CombinedReportViewModel
                {
                    DemographicReport = new DemographicReportViewModel
                    {
                        CampusOptions = GetCampusOptions(),
                        AgeGroupOptions = GetAgeGroupOptions(),
                        SexOptions = GetSexOptions(),
                        GenderIdentityOptions = GetGenderIdentityOptions(),
                        UserTypeOptions = GetUserTypeOptions(),
                    },
                    ModuleReport = model
                };
                return View("Index", combinedModelException);
            }
        }

        // **Helper Methods for Dropdown Options**
        private List<SelectListItem> GetCampusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "sta-mesa", Text = "Sta. Mesa, Manila (Main Campus)" }
                // Add more campuses as needed
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
                // Add more sex options if necessary
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
                // Add more gender identities as needed
            };
        }

        private List<SelectListItem> GetUserTypeOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "student", Text = "Student" },
                new SelectListItem { Value = "employee", Text = "Employee" }
                // Add more user types if necessary
            };
        }

        private async Task<List<SelectListItem>> GetModuleOptionsAsync()
        {
            var modules = await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published)
                .Select(m => new SelectListItem
                {
                    Value = m.ModuleId.ToString(),
                    Text = m.Title
                })
                .ToListAsync();

            // Insert default option at the beginning
            modules.Insert(0, new SelectListItem { Value = "", Text = "Select a Module", Disabled = true, Selected = true });

            return modules;
        }


        // **Generate CSV Method for Demographic Report**
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

            return System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        // **Generate PDF Method for Demographic Report**
        private byte[] GeneratePdf(List<ApplicationUser> users, DemographicReportViewModel model)
        {
            // Configure QuestPDF license
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            using var ms = new System.IO.MemoryStream();
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(column =>
                    {
                        column.Item().Text("Demographic Report").FontSize(16).Bold();
                        column.Item().LineHorizontal(1);
                        column.Item().Height(10); // Replaces Spacing(10)

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
            static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => container.Padding(5);
        }

        // **Helper Method to Calculate Age**
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        // **Helper Method to Determine Age Group**
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
