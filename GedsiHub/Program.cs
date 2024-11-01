// Program.cs
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Repositories;
using GedsiHub.Services;
using GedsiHub.Seeders;
using GedsiHub.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using OfficeOpenXml;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. Configure Logging with Serilog
// ========================================
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// ========================================
// 2. Configure Database and Identity
// ========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ========================================
// 3. Configure External Services and API Clients
// ========================================
builder.Services.AddHttpClient<WatershedApiService>(client =>
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// ========================================
// 4. Register Application Services
// ========================================
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<XApiService>(); // Register XApiService
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddHostedService<StaleActiveUserCleanupService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<EmailSender>(); // Ensure EmailSender is registered
builder.Services.AddTransient<CertificateService>();

builder.Services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();

// Correctly register IHubContext for SignalR
builder.Services.AddSignalR();

// ========================================
// 5. Configure Razor Pages and MVC with Global Authorization
// ========================================
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddRazorPagesOptions(options =>
{
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
});

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// ========================================
// 6. Configure DinkToPdf for PDF Generation
// ========================================
var wkhtmltoxPath = Path.Combine(builder.Environment.WebRootPath, "lib", "wkhtmltox", "bin");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    SetDllDirectory(wkhtmltoxPath);
}

var converter = new SynchronizedConverter(new PdfTools());
builder.Services.AddSingleton(typeof(IConverter), converter);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetDllDirectory(string lpPathName);

// ========================================
// 7. Configure Middleware and Seed Data
// ========================================
var app = builder.Build();

// Configure EPPlus License
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Seed Roles and Initial Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        var roleSeeder = new RoleSeeder(roleManager);
        await roleSeeder.SeedRolesAsync();

        await SeedAdminUser(userManager, "admin@gado.com", "AdminPassword123!");
        await SeedStudentUser(userManager, context, "student@gado.com", "StudentPassword123!");
        await SeedEmployeeUser(userManager, context, "employee@gado.com", "EmployeePassword123!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles or users.");
    }
}

// ========================================
// 8. Configure Middleware Pipeline
// ========================================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ========================================
// 9. Map Endpoints
// ========================================
app.MapHub<AnalyticsHub>("/analyticsHub"); // Map SignalR Hub
app.UseMiddleware<XApiEnrichmentMiddleware>(); // Add custom middleware
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "chatbot",
    pattern: "Chatbot",
    defaults: new { controller = "Chatbot", action = "Index" });
app.MapRazorPages();
app.Run();

// ========================================
// 10. Helper Methods for Seeding Users
// ========================================
async Task SeedAdminUser(UserManager<ApplicationUser> userManager, string adminEmail, string adminPassword)
{
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Admin");
        }
    }
}

async Task SeedStudentUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string studentEmail, string studentPassword)
{
    var studentUser = await userManager.FindByEmailAsync(studentEmail);
    if (studentUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = studentEmail,
            Email = studentEmail,
            FirstName = "Student",
            LastName = "User",
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newUser, studentPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Student");

            var student = new Student
            {
                UserId = newUser.Id,
                Year = 1,
                Section = "A",
                CollegeDeptId = 1,
                CourseId = 1
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();
        }
    }
}

async Task SeedEmployeeUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string employeeEmail, string employeePassword)
{
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
    if (employeeUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            FirstName = "Employee",
            LastName = "User",
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newUser, employeePassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Employee");

            var employee = new Employee
            {
                UserId = newUser.Id,
                EmployeeType = "Full-Time",
                EmploymentStatus = "Active",
                BranchOfficeSectionUnit = "HR",
                Position = "Manager",
                Sector = "Human Resources"
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }
    }
}
