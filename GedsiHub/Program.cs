// Program.cs
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Models.Quiz;
using GedsiHub.Repositories;
using GedsiHub.Services;
using GedsiHub.Services.Interfaces;
using GedsiHub.Seeders;
using GedsiHub.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
// 1. Configure Database and Identity
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
// 2. Configure External Services and API Clients
// ========================================
builder.Services.AddHttpClient<WatershedApiService>(client =>
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// ========================================
// 3. Register Application Services
// ========================================
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IReportManagementService, ReportManagementService>();
builder.Services.AddScoped<IUserDeletionService, UserDeletionService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<XApiService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddHostedService<StaleActiveUserCleanupService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddTransient<CertificateService>();
builder.Services.AddScoped<IExam<Exam>, ExamService<Exam>>();
builder.Services.AddScoped<IQuestion<Question>, QuestionService<Question>>();
builder.Services.AddScoped<IResult<QuizResult>, ResultService<QuizResult>>();
builder.Services.AddScoped(typeof(IChoice<>), typeof(ChoiceService<>));
builder.Services.AddScoped(typeof(IResult<>), typeof(ResultService<>));
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddTransient<ExcelParserService>();


builder.Services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();

// Correctly register IHubContext for SignalR
builder.Services.AddSignalR();

// Configure logging (this is optional, as the default settings include Console and Debug)
builder.Logging.ClearProviders(); // Optional: Clear default providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging (logs to Visual Studio output)

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Makes the cookie accessible only by the server
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

// ========================================
// 4. Configure Razor Pages and MVC with Global Authorization
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
    options.AccessDeniedPath = "/Error/403";
});

// ========================================
// 5. Configure DinkToPdf for PDF Generation
// ========================================
var wkhtmltoxPath = Path.Combine(builder.Environment.WebRootPath, "wwwroot", "wwwroot", "lib", "wkhtmltox", "bin");

Console.WriteLine($"wkhtmltox path: {wkhtmltoxPath}");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    SetDllDirectory(wkhtmltoxPath);
}

var converter = new SynchronizedConverter(new PdfTools());
builder.Services.AddSingleton(typeof(IConverter), converter);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetDllDirectory(string lpPathName);

// ========================================
// 6. Configure Middleware and Seed Data
// ========================================
var app = builder.Build();

// Configure EPPlus License
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Program.cs

// Seed Roles and Initial Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<RoleSeeder>>();

        // Pass the logger to the RoleSeeder
        var roleSeeder = new RoleSeeder(roleManager, logger);
        await roleSeeder.SeedRolesAsync(); // This should create "Admin", "Student", "Employee" roles

        // Now that roles exist, you can safely create users and assign roles
        await SeedAdminUser(userManager, context, logger, "admin@gado.com", "AdminPassword123!");
        await SeedStudentUser(userManager, context, logger, "student@gado.com", "StudentPassword123!");
        await SeedEmployeeUser(userManager, context, logger, "employee@gado.com", "EmployeePassword123!");

        // Seed the anonymous user
        await SeedAnonymousUser(userManager, logger);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles or users.");
    }
}


// ========================================
// 7. Configure Middleware Pipeline
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



app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<GedsiHub.Middleware.EnsureProfileCompleteMiddleware>();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

// ========================================
// 8. Map Endpoints
// ========================================
app.MapHub<AnalyticsHub>("/analyticsHub");
app.MapHub<NotificationHub>("/notificationHub");
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
// 9. Helper Methods for Seeding Users
// ========================================
async Task SeedAdminUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger logger, string adminEmail, string adminPassword)
{
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Alexander",
            LastName = "Grant",
            MiddleName = "James",
            Suffix = null,
            Honorifics = "Mr.",
            LivedName = null,
            Sex = "Male",
            GenderIdentity = "Cisgender",
            Pronouns = "He/Him",
            IsMemberOfIndigenousCommunity = false,
            IsDisabled = false,
            DateOfBirth = new DateTime(1980, 1, 1),
            Campus = "Sta. Mesa, Manila (Main Campus)",
            ProfilePicturePath = null,
            IsActive = true,
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(newUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Admin");

            var admin = new Admin
            {
                UserId = newUser.Id,
                AdminName = "Primary Admin"
            };
            context.Admins.Add(admin);
            await context.SaveChangesAsync();

            logger.LogInformation("Admin user seeded successfully.");
        }
        else
        {
            logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        logger.LogInformation("Admin user already exists.");
    }
}

async Task SeedStudentUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger logger, string studentEmail, string studentPassword)
{
    var studentUser = await userManager.FindByEmailAsync(studentEmail);
    if (studentUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = studentEmail,
            Email = studentEmail,
            FirstName = "Sophia",
            LastName = "Lopez",
            MiddleName = "Marie",
            Suffix = null,
            Honorifics = "Ms.",
            LivedName = "Sophie",
            Sex = "Female",
            GenderIdentity = "Cisgender",
            Pronouns = "She/Her",
            IsMemberOfIndigenousCommunity = false,
            IsDisabled = false,
            DateOfBirth = new DateTime(2003, 5, 15),
            Campus = "Sta. Mesa, Manila (Main Campus)",
            ProfilePicturePath = null,
            IsActive = true,
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(newUser, studentPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Student");

            var student = new Student
            {
                UserId = newUser.Id,
                Year = 2,
                Section = "1N",
                CollegeDeptId = 1,
                CourseId = 2
            };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            logger.LogInformation("Student user seeded successfully.");
        }
        else
        {
            logger.LogError("Failed to create student user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        logger.LogInformation("Student user already exists.");
    }
}

async Task SeedEmployeeUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger logger, string employeeEmail, string employeePassword)
{
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
    if (employeeUser == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            FirstName = "Emily",
            LastName = "Chen",
            MiddleName = "Grace",
            Suffix = null,
            Honorifics = "Ms.",
            LivedName = null,
            Sex = "Female",
            GenderIdentity = "Cisgender",
            Pronouns = "She/Her",
            IsMemberOfIndigenousCommunity = false,
            IsDisabled = false,
            DateOfBirth = new DateTime(1990, 8, 12),
            Campus = "Sta. Mesa, Manila (Main Campus)",
            ProfilePicturePath = null,
            IsActive = true,
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
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
                BranchOfficeSectionUnit = "Finance Department",
                Position = "Accountant",
                Sector = "Accounting and Finance"
            };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            logger.LogInformation("Employee user seeded successfully.");
        }
        else
        {
            logger.LogError("Failed to create employee user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        logger.LogInformation("Employee user already exists.");
    }
}

async Task SeedAnonymousUser(UserManager<ApplicationUser> userManager, ILogger logger)
{
    const string anonymousUserId = "AnonymousUserId";
    const string anonymousUserPassword = "AnonUser@2024";
    var anonymousUser = await userManager.FindByIdAsync(anonymousUserId);
    if (anonymousUser == null)
    {
        var newUser = new ApplicationUser
        {
            Id = anonymousUserId, // Explicitly set the ID for consistency
            UserName = "Anonymous",
            Email = "anonymous@gado.com",
            FirstName = "Deleted",
            LastName = "User",
            IsActive = false,
            EmailConfirmed = true, // Ensure it's marked as confirmed
            LockoutEnabled = true // Prevent the account from being logged into
        };

        var result = await userManager.CreateAsync(newUser, anonymousUserPassword); // Predefined strong password
        if (result.Succeeded)
        {
            logger.LogInformation("Anonymous user seeded successfully.");
        }
        else
        {
            logger.LogError("Failed to create anonymous user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        logger.LogInformation("Anonymous user already exists.");
    }
}
