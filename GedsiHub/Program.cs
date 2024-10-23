using GedsiHub.Data;
using GedsiHub.Services;
using GedsiHub.Models;
using GedsiHub.Seeders;
using GedsiHub.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
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
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<EmailSender>();
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
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roleSeeder = new RoleSeeder(roleManager);
        await roleSeeder.SeedRolesAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles.");
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
