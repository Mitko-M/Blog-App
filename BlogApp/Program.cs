using Microsoft.AspNetCore.Mvc;

/// <summary>
/// BlogApp Application Entry Point
/// 
/// This file configures the ASP.NET Core application pipeline:
/// 1. Services Configuration: Registers database, identity, and business logic services
/// 2. Middleware Pipeline: Defines request processing order
/// 3. Routing: Maps URL patterns to controllers and actions
/// 
/// See docs/SETUP.md and docs/DEVELOPMENT.md for more information.
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// SERVICE CONFIGURATION (Dependency Injection)
// ============================================================================

// Configure Entity Framework Core and database context
// Extension method defined in BlogApp/Extensions/ServiceCollectionExtension.cs
builder.Services.AddAppDbContext(builder.Configuration);

// Configure ASP.NET Core Identity for user authentication/authorization
// Sets up password hashing, role management, cookie authentication
builder.Services.AddAppIdentity();

// Configure MVC controllers and views
// Automatically enables CSRF token validation on all state-modifying operations
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

// Register application business logic services (IPostService, ICommentService, etc.)
// All services use Scoped lifetime (new instance per HTTP request)
// Extension method defined in BlogApp/Extensions/ServiceCollectionExtension.cs
builder.Services.AddAppServices();

// ============================================================================
// BUILD THE APPLICATION
// ============================================================================

var app = builder.Build();

// ============================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// ============================================================================

if (app.Environment.IsDevelopment())
{
    // Development: Show detailed error pages and enable database migrations UI
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    // Production: Use generic error page and handle missing pages gracefully
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

    // Enable HSTS (HTTP Strict-Transport-Security) in production
    // Forces HTTPS for enhanced security
    app.UseHsts();
}

// Redirect HTTP to HTTPS (security best practice)
app.UseHttpsRedirection();

// Enable serving static files (CSS, JS, images, etc.) from wwwroot/
app.UseStaticFiles();

// Enable routing - must come before authentication/authorization
app.UseRouting();

// Authenticate user based on cookie/claims
// Must come before UseAuthorization
app.UseAuthentication();

// Authorize based on [Authorize] attributes and policies
// Must come after UseAuthentication
app.UseAuthorization();

// ============================================================================
// ROUTE CONFIGURATION
// ============================================================================
// Routes are evaluated in order - more specific routes should come first

app.UseEndpoints(endpoints =>
{
    // Admin Area Routes
    // Pattern: /Admin/{controller=Home}/{action=Index}/{id?}
    // Example: /Admin/User/Manage
    endpoints.MapAreaControllerRoute(
        name: "Admin Area",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    // User Area Routes
    // Pattern: /User/{controller}/{action}/{id?}
    // Example: /User/Account/Login
    endpoints.MapAreaControllerRoute(
        name: "User Area",
        areaName: "User",
        pattern: "User/{controller}/{action}/{id?}");

    // Post Details Route (SEO-friendly URL with title)
    // Pattern: /Post/Details/{id}/{title}
    // Example: /Post/Details/1/my-first-blog-post
    endpoints.MapControllerRoute(
        name: "Post Details",
        pattern: "/Post/Details/{id}/{title}",
        defaults: new { Controller = "Post", Action = "Details" });

    // Default Route
    // Pattern: /{controller=Home}/{action=All}/{id?}
    // Default controller: HomeController
    // Default action: All (blog posts listing)
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=All}/{id?}");

    // Razor Pages routing (for future Identity pages)
    endpoints.MapRazorPages();
});

// ============================================================================
// RUN THE APPLICATION
// ============================================================================

// Start listening for HTTP requests asynchronously
// The application will run until it receives a shutdown signal
await app.RunAsync();
