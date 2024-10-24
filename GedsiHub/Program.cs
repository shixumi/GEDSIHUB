using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Services;
using GedsiHub.Seeders;
using GedsiHub.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.SignalR;

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
    // Add other sinks as needed (e.g., File, Seq)
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
// 3. Register Application Services
// ========================================

// Register EmailSender and CertificateService
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, GedsiHub.Services.EmailSender>();
builder.Services.AddTransient<CertificateService>();


// Register Analytics Services
builder.Services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddHostedService<StaleActiveUserCleanupService>();

// Register SignalR
builder.Services.AddSignalR();

// Register Controllers and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ========================================
// 4. Configure QuestPDF License
// ========================================
QuestPDF.Settings.License = LicenseType.Community; // Sets the QuestPDF license to the community version

var app = builder.Build();

// ========================================
// 5. Seed Roles and Initial Data
// ========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Retrieve services needed for seeding
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Seed roles using your RoleSeeder
        var roleSeeder = new RoleSeeder(roleManager);
        await roleSeeder.SeedRolesAsync();

        // Seed Admin user
        await SeedAdminUser(userManager, "admin@gado.com", "AdminPassword123!");

        // Seed separate Student and Employee users
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
// 6. Configure Middleware Pipeline
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
// 7. Map Endpoints
// ========================================

// Map SignalR Hubs
app.MapHub<AnalyticsHub>("/analyticsHub");

// Map Controller Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Specific Route for Chatbot
app.MapControllerRoute(
    name: "chatbot",
    pattern: "Chatbot",
    defaults: new { controller = "Chatbot", action = "Index" });

// Map Razor Pages
app.MapRazorPages();

// Run the Application
app.Run();

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
            IsActive = true, // Set as active
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
            IsActive = true, // Set as active
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newUser, studentPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Student");

            // Create a student record
            var student = new Student
            {
                UserId = newUser.Id,
                Year = 1,
                Section = "A",
                CollegeDeptId = 1, // Assuming you have this data in CollegeDepartment
                CourseId = 1 // Assuming you have this data in Course
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
            IsActive = true, // Set as active
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newUser, employeePassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Employee");

            // Create an employee record
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
