using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GedsiHub.Services
{
    public class ReportGenerationService : IReportGenerationService
    {
        private readonly AnalyticsService _analyticsService;
        private readonly ILogger<ReportGenerationService> _logger;

        public ReportGenerationService(AnalyticsService analyticsService, ILogger<ReportGenerationService> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        public async Task<byte[]> GenerateModuleCsvAsync(Module module, ModuleReportViewModel model, string userName)
        {
            try
            {
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Metric,Value");

                if (model.IncludeCompletionRate)
                {
                    var completionRate = await _analyticsService.GetModuleCompletionRateAsync(module.ModuleId);
                    csvBuilder.AppendLine($"Completion Rate,{completionRate}%");
                }

                if (model.IncludeAverageQuizScore)
                {
                    var averageQuizScore = await _analyticsService.GetAverageQuizScoreAsync(module.ModuleId);
                    csvBuilder.AppendLine($"Average Quiz Score,{averageQuizScore}");
                }

                int certificatesIssued = 0;

                if (model.IncludeCertificatesIssued)
                {
                    certificatesIssued = await _analyticsService.GetCertificateIssuanceRateAsync(module.ModuleId);
                    csvBuilder.AppendLine($"Certificates Issued,{certificatesIssued}");
                }

                return Encoding.UTF8.GetBytes(csvBuilder.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating CSV for module report.");
                throw;
            }
        }

        public async Task<byte[]> GenerateModulePdfAsync(Module module, ModuleReportViewModel model, string userName)
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;

                // Pre-fetch all required data asynchronously
                double? completionRate = null;
                double? averageQuizScore = null;
                int certificatesIssued = 0;

                if (model.IncludeCompletionRate)
                {
                    completionRate = await _analyticsService.GetModuleCompletionRateAsync(module.ModuleId);
                }

                if (model.IncludeAverageQuizScore)
                {
                    averageQuizScore = await _analyticsService.GetAverageQuizScoreAsync(module.ModuleId);
                }

                if (model.IncludeCertificatesIssued)
                {
                    certificatesIssued = await _analyticsService.GetCertificateIssuanceRateAsync(module.ModuleId);
                }

                using var ms = new MemoryStream();
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Content().Column(column =>
                        {
                            column.Item().Text("Module Report").FontSize(16).Bold().AlignCenter();
                            column.Item().LineHorizontal(1);
                            column.Item().Height(10);

                            column.Item().Text($"Module: {module.Title}").FontSize(14).Bold();

                            if (model.IncludeCompletionRate && completionRate.HasValue)
                            {
                                column.Item().Text($"Completion Rate: {completionRate.Value}%");
                            }

                            if (model.IncludeAverageQuizScore && averageQuizScore.HasValue)
                            {
                                column.Item().Text($"Average Quiz Score: {averageQuizScore.Value}");
                            }

                            if (model.IncludeCertificatesIssued)
                            {
                                column.Item().Text($"Certificates Issued: {certificatesIssued}");
                            }
                        });
                    });
                });

                document.GeneratePdf(ms);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for module report.");
                throw;
            }
        }
    }
}