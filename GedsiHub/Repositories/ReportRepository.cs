using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GedsiHub.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportRepository> _logger;

        public ReportRepository(ApplicationDbContext context, ILogger<ReportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync(DemographicReportViewModel filters)
        {
            // Start building the query from the Users table
            var query = _context.Users.AsQueryable();

            // Apply the date range filter
            if (filters.DateRange == "7days")
            {
                query = query.Where(u => u.CreatedDate >= DateTime.UtcNow.AddDays(-7));
            }
            else if (filters.DateRange == "28days")
            {
                query = query.Where(u => u.CreatedDate >= DateTime.UtcNow.AddDays(-28));
            }
            else if (filters.DateRange == "60days")
            {
                query = query.Where(u => u.CreatedDate >= DateTime.UtcNow.AddDays(-60));
            }
            else if (filters.DateRange == "custom" && filters.CustomStartDate.HasValue && filters.CustomEndDate.HasValue)
            {
                // Log the custom date range values
                _logger.LogInformation($"Custom Start Date: {filters.CustomStartDate.Value}");
                _logger.LogInformation($"Custom End Date: {filters.CustomEndDate.Value}");

                // Apply the custom date range filter
                query = query.Where(u => u.CreatedDate >= filters.CustomStartDate.Value && u.CreatedDate <= filters.CustomEndDate.Value);
            }
            else
            {
                _logger.LogWarning("Custom date range not provided or invalid.");
            }

            // Apply the campus filter (case-insensitive)
            if (!string.IsNullOrEmpty(filters.Campus))
            {
                if (filters.Campus.Equals("sta-mesa", StringComparison.OrdinalIgnoreCase))
                {
                    // Exact match for the "Sta. Mesa, Manila (Main Campus)" campus
                    query = query.Where(u => u.Campus == "Sta. Mesa, Manila (Main Campus)");
                }
                else
                {
                    // Use a case-insensitive contains check for other campuses
                    query = query.Where(u => u.Campus.ToLower().Contains(filters.Campus.ToLower()));
                }
            }

            // Apply the sex filter (case-insensitive)
            if (!string.IsNullOrEmpty(filters.Sex))
            {
                query = query.Where(u => u.Sex.ToLower() == filters.Sex.ToLower());
            }

            // Apply the gender identity filter (case-insensitive)
            if (!string.IsNullOrEmpty(filters.GenderIdentity))
            {
                query = query.Where(u => u.GenderIdentity.ToLower() == filters.GenderIdentity.ToLower());
            }

            // Apply the user type filter
            if (!string.IsNullOrEmpty(filters.UserType))
            {
                if (filters.UserType == "student")
                {
                    query = query.Where(u => u.Student != null);
                }
                else if (filters.UserType == "employee")
                {
                    query = query.Where(u => u.Employee != null);
                }
            }

            // Fetch the data from the database
            var users = await query.ToListAsync();

            // Apply in-memory filtering for the age group, if specified
            if (!string.IsNullOrEmpty(filters.AgeGroup))
            {
                users = users.Where(u => DetermineAgeGroup(CalculateAge(u.DateOfBirth)) == filters.AgeGroup).ToList();
            }

            return users;
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