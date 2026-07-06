# Troubleshooting Guide

Solutions for common issues encountered when developing, deploying, or using BlogApp.

## Table of Contents

- [Development Issues](#development-issues)
- [Database Issues](#database-issues)
- [Build & Compilation](#build--compilation)
- [Runtime Issues](#runtime-issues)
- [Authentication & Authorization](#authentication--authorization)
- [Performance Issues](#performance-issues)
- [Deployment Issues](#deployment-issues)
- [Getting Help](#getting-help)

---

## Development Issues

### "Cannot find SQL Server"

**Symptoms:**
- Connection refused errors
- `(local)` or `(localdb)` not found

**Solutions:**

1. Verify SQL Server is running:
   ```bash
   SqlLocalDB info
   SqlLocalDB start mssqllocaldb
   ```

2. Check connection string:
   ```bash
   # Good examples
   Server=(localdb)\mssqllocaldb;Database=BlogAppDb;Trusted_Connection=true;
   Server=YOUR_MACHINE_NAME\SQLEXPRESS;Database=BlogAppDb;Trusted_Connection=true;
   Server=tcp:myserver.database.windows.net,1433;Initial Catalog=BlogAppDb;User ID=admin;Password=***;
   ```

3. Test connection:
   ```bash
   # SQL Server Management Studio
   # Connect to (localdb)\mssqllocaldb
   ```

---

### "Port 5001 already in use"

**Symptoms:**
```
fail: Microsoft.AspNetCore.Server.Kestrel[0]
	  Unable to start Kestrel.
System.IO.IOException: Failed to bind to address...
```

**Solutions:**

1. Run on different port:
   ```bash
   dotnet run --project BlogApp --urls "https://localhost:5002"
   ```

2. Find and kill process using port:
   ```bash
   # Windows
   netstat -ano | findstr :5001
   taskkill /PID <PID> /F

   # Linux/macOS
   lsof -i :5001
   kill -9 <PID>
   ```

3. Change default port in Properties/launchSettings.json:
   ```json
   "applicationUrl": "https://localhost:5002;http://localhost:5003"
   ```

---

### "Cannot find NuGet package"

**Symptoms:**
- Package restore fails
- Assembly not found after building

**Solutions:**

1. Clear NuGet cache:
   ```bash
   dotnet nuget locals all --clear
   dotnet restore
   ```

2. Specify NuGet source explicitly:
   ```bash
   dotnet restore --source "https://api.nuget.org/v3/index.json"
   ```

3. Check internet connection and proxy settings

4. Check nuget.config file exists in solution root:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
	 <packageSources>
	   <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
	 </packageSources>
   </configuration>
   ```

---

### "Changes not reflected after code edit"

**Symptoms:**
- Modified code not appearing after rebuild
- Old code still running

**Solutions:**

1. Full clean and rebuild:
   ```bash
   dotnet clean
   dotnet build
   dotnet run --project BlogApp
   ```

2. Stop running process:
   - Ctrl+C to stop application
   - Wait a few seconds
   - Rebuild
   - Run again

3. Delete bin/obj directories:
   ```bash
   # Windows
   rmdir /s /q bin obj

   # Linux/macOS
   rm -rf bin obj
   ```

4. Restart Visual Studio (if using VS)

---

## Database Issues

### "No migrations found"

**Symptoms:**
```
Unable to create an object of type 'BlogAppDbContext'. For the different patterns supported at design time
```

**Solutions:**

1. Verify BlogApp.Infrastructure project has migrations:
   ```bash
   dotnet ef migrations list --project BlogApp.Infrastructure
   ```

2. If no migrations, create initial:
   ```bash
   dotnet ef migrations add "InitialMigration" --project BlogApp.Infrastructure --startup-project BlogApp
   ```

3. Apply migrations:
   ```bash
   dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
   ```

---

### "Migration migration fails or hangs"

**Symptoms:**
- Migration script stuck
- Timeout errors
- Deadlock messages

**Solutions:**

1. Check SQL Server for blocking processes:
   ```sql
   -- Find blocking queries
   SELECT blocking_session_id, session_id FROM sys.dm_exec_requests WHERE blocking_session_id <> 0;

   -- Kill blocking session
   KILL <session_id>;
   ```

2. Increase migration timeout:
   ```bash
   dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp --verbose -- --command-timeout 300
   ```

3. Apply migration step by step:
   ```bash
   # List all migrations
   dotnet ef migrations list --project BlogApp.Infrastructure

   # Apply specific migration
   dotnet ef database update "SpecificMigrationName" --project BlogApp.Infrastructure
   ```

---

### "Cannot login after migration"

**Symptoms:**
- Default admin credentials don't work
- Login page shows but authentication fails

**Solutions:**

1. Verify migrations were applied:
   ```bash
   dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
   ```

2. Check seed data in migration file:
   ```csharp
   // BlogApp.Infrastructure/Migrations/[date]_SeedingDataWithAdmin.cs
   // Look for admin user seed
   ```

3. Reset database and apply fresh:
   ```bash
   dotnet ef database drop --project BlogApp.Infrastructure --startup-project BlogApp
   dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
   ```

4. Manually create admin user:
   ```bash
   # Use User Secrets to manage credentials
   dotnet user-secrets init --project BlogApp
   dotnet user-secrets set "AdminEmail" "admin@example.com"
   dotnet user-secrets set "AdminPassword" "TempPassword123!"
   ```

---

### "Database locked or access denied"

**Symptoms:**
```
Access denied for user 'SA'@'localhost'
Database is locked
```

**Solutions:**

1. Check SQL Server is running and accessible:
   ```bash
   sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT 1"
   ```

2. Verify credentials in connection string:
   ```json
   {
	 "ConnectionStrings": {
	   "DefaultConnection": "Server=...;User Id=correct_user;Password=correct_password;"
	 }
   }
   ```

3. Check file permissions (Linux/Docker):
   ```bash
   chmod -R 755 /var/opt/mssql/data
   chown -R mssql:mssql /var/opt/mssql
   ```

---

## Build & Compilation

### "Build fails with CS0246: Cannot find namespace"

**Symptoms:**
```
Error CS0246: The type or namespace name 'BlogApp' could not be found
```

**Solutions:**

1. Restore packages:
   ```bash
   dotnet restore
   ```

2. Add missing using statement:
   ```csharp
   using BlogApp.Core.Contracts;
   using BlogApp.Infrastructure.Data;
   ```

3. Rebuild entire solution:
   ```bash
   dotnet clean
   dotnet build --no-restore
   ```

4. Check project references:
   ```bash
   # BlogApp.csproj should reference:
   # BlogApp.Core.csproj
   ```

---

### "Build fails with multiple warnings"

**Symptoms:**
- Nullability warnings
- Obsolete API warnings
- Code analysis warnings

**Solutions:**

1. Fix nullable reference issues:
   ```csharp
   // ✓ Good
   public string? Name { get; set; }
   public string Description { get; set; } = string.Empty;

   // ✗ Bad
   public string Name { get; set; }
   ```

2. Fix obsolete API usage:
   - Check compiler warning message
   - Use suggested replacement
   - Update to newer API

3. Suppress warnings (if intentional):
   ```csharp
   #pragma warning disable CS0618
   var obsoleteMethod = SomeObsoleteMethod();
   #pragma warning restore CS0618
   ```

4. Run code formatter:
   ```bash
   dotnet format
   ```

---

### "Out of memory during build"

**Symptoms:**
- Build process terminates unexpectedly
- OutOfMemoryException

**Solutions:**

1. Close other applications to free memory

2. Rebuild incrementally:
   ```bash
   dotnet clean --project BlogApp
   dotnet build --project BlogApp
   ```

3. Increase MSBuild memory (Visual Studio):
   - Tools → Options → Projects and Solutions → Build and Run
   - Set appropriate memory limit

---

## Runtime Issues

### "Null reference exception at startup"

**Symptoms:**
```
System.NullReferenceException: Object reference not set to an instance of an object.
at BlogApp.Program.Main()
```

**Solutions:**

1. Check dependency injection registration:
   ```csharp
   // BlogApp/Extensions/ServiceCollectionExtension.cs
   // Ensure all services are registered
   services.AddScoped<IPostService, PostService>();
   ```

2. Check constructor parameters:
   ```csharp
   // Service constructor
   public PostService(BlogAppDbContext context)
   {
	   _context = context ?? throw new ArgumentNullException(nameof(context));
   }
   ```

3. Add null checks:
   ```csharp
   if (service == null)
	   throw new InvalidOperationException("Service not registered");
   ```

---

### "Invalid model state after form submission"

**Symptoms:**
- Form validation fails
- ModelState.IsValid == false
- Validation error messages not showing

**Solutions:**

1. Add validation to model:
   ```csharp
   public class AddPostFormModel
   {
	   [Required(ErrorMessage = "Title is required")]
	   [StringLength(50, MinimumLength = 5)]
	   public string Title { get; set; }
   }
   ```

2. Display validation messages in view:
   ```html
   @if (!ViewData.ModelState.IsValid)
   {
	   <div class="alert alert-danger">
		   @Html.ValidationSummary()
	   </div>
   }
   ```

3. Debug model binder:
   ```csharp
   [HttpPost]
   public IActionResult Create(AddPostFormModel model)
   {
	   if (!ModelState.IsValid)
	   {
		   var errors = ModelState.Values.SelectMany(v => v.Errors);
		   foreach (var error in errors)
			   System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
	   }
	   // ...
   }
   ```

---

### "Views not found (404)"

**Symptoms:**
```
InvalidOperationException: The view 'Index' was not found
```

**Solutions:**

1. Check view file exists:
   - View must be in `Views/{ControllerName}/{ActionName}.cshtml`
   - Example: `Views/Post/Create.cshtml`

2. Verify view file name matches action:
   ```csharp
   public IActionResult Create() // Returns Create.cshtml
   public IActionResult Edit()   // Returns Edit.cshtml
   ```

3. Check Areas views:
   - Admin views: `Areas/Admin/Views/{ControllerName}/{ActionName}.cshtml`
   - User views: `Areas/User/Views/{ControllerName}/{ActionName}.cshtml`

4. Rebuild project:
   ```bash
   dotnet clean
   dotnet build
   ```

---

## Authentication & Authorization

### "Login page shows but login fails"

**Symptoms:**
- Login page loads
- Credentials entered but login fails
- No error message or unclear error

**Solutions:**

1. Check identity configuration:
   ```csharp
   // Verify in Program.cs
   builder.Services.AddAppIdentity();
   app.UseAuthentication();
   app.UseAuthorization();
   ```

2. Verify user exists in database:
   ```sql
   SELECT Id, UserName, Email FROM AspNetUsers WHERE UserName = 'admin';
   ```

3. Check password hashing:
   ```csharp
   // Use UserManager to verify password
   var user = await userManager.FindByNameAsync(username);
   var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
   ```

4. Check application cookie settings:
   ```csharp
   services.ConfigureApplicationCookie(options =>
   {
	   options.LoginPath = "/User/Account/Login";
	   options.LogoutPath = "/User/Account/Logout";
	   options.AccessDeniedPath = "/User/Access/Index";
   });
   ```

---

### "Access denied (403) on protected pages"

**Symptoms:**
- Authenticated users get 403 error
- Admin pages inaccessible

**Solutions:**

1. Check authorization attribute:
   ```csharp
   [Authorize]                      // Any authenticated user
   [Authorize(Roles = "Admin")]     // Admin role required
   public IActionResult Edit() { }
   ```

2. Verify user has correct role:
   ```sql
   SELECT ur.UserId, r.Name FROM AspNetUserRoles ur
   JOIN AspNetRoles r ON ur.RoleId = r.Id
   WHERE ur.UserId = 'user-id';
   ```

3. Check role claims are loaded:
   ```csharp
   var roles = User.FindAll(ClaimTypes.Role);
   System.Diagnostics.Debug.WriteLine($"Roles: {string.Join(", ", roles.Select(r => r.Value))}");
   ```

---

### "Logout not working"

**Symptoms:**
- User logged out but still authenticated
- Can access protected pages after logout

**Solutions:**

1. Verify logout action:
   ```csharp
   [HttpPost]
   public async Task<IActionResult> Logout()
   {
	   await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
	   return RedirectToAction("All", "Home");
   }
   ```

2. Check cookie is cleared:
   - Open browser Developer Tools
   - Application → Cookies
   - Verify `.AspNetCore.Identity.Application` is removed

3. Clear browser cache:
   - Ctrl+Shift+Delete
   - Clear cookies and cache

---

## Performance Issues

### "Application slow or unresponsive"

**Symptoms:**
- Pages take long time to load
- High CPU usage
- Database queries slow

**Solutions:**

1. Profile with Application Insights (if deployed):
   ```csharp
   services.AddApplicationInsightsTelemetry();
   ```

2. Check for N+1 queries:
   ```csharp
   // ✗ Bad - N+1 query
   var posts = _context.Posts.ToList();
   foreach (var post in posts)
	   var author = _context.Users.Find(post.UserId);

   // ✓ Good - Single query with Include
   var posts = _context.Posts
	   .Include(p => p.User)
	   .ToList();
   ```

3. Add pagination:
   ```csharp
   var posts = await _context.Posts
	   .Skip((page - 1) * pageSize)
	   .Take(pageSize)
	   .ToListAsync();
   ```

4. Enable query logging:
   ```csharp
   optionsBuilder.LogTo(Console.WriteLine);
   ```

---

### "Memory leak or growing memory usage"

**Symptoms:**
- Task Manager shows increasing memory
- Application crashes after running long

**Solutions:**

1. Dispose of resources:
   ```csharp
   using (var context = new BlogAppDbContext())
   {
	   // Use context
   } // Automatically disposed
   ```

2. Check for circular references:
   - Post → User → Posts (circular)
   - Can cause memory issues

3. Monitor with Memory Profiler:
   - Visual Studio → Debug → Windows → Memory Profiler
   - Take snapshots and compare

---

## Deployment Issues

### "Application won't start in production"

**Symptoms:**
- IIS/Docker shows startup error
- Service won't start
- No clear error message

**Solutions:**

1. Check logs:
   - IIS: `C:\inetpub\logs\LogFiles\`
   - Docker: `docker logs container_name`
   - Windows Event Viewer

2. Verify configuration:
   ```json
   // Ensure appsettings.Production.json exists
   {
	 "ConnectionStrings": { /* Valid connection string */ },
	 "Logging": { /* Valid logging config */ }
   }
   ```

3. Check dependencies:
   - SQL Server is accessible
   - All referenced assemblies present
   - .NET runtime matches version

---

### "Database connection fails in production"

**Symptoms:**
```
Cannot open database "BlogAppDb" requested by the login
```

**Solutions:**

1. Verify connection string:
   ```bash
   # Test connection
   sqlcmd -S prod-server -U sa -P password -d BlogAppDb -Q "SELECT 1"
   ```

2. Check firewall:
   ```bash
   # Allow SQL Server port
   Windows Firewall → Allow an app → SQL Server
   ```

3. Verify database exists:
   ```sql
   SELECT name FROM sys.databases WHERE name = 'BlogAppDb';
   ```

4. Check user permissions:
   ```sql
   GRANT CONNECT, SELECT, INSERT, UPDATE, DELETE ON DATABASE::BlogAppDb TO [blog_user];
   ```

---

### "SSL/HTTPS certificate errors"

**Symptoms:**
```
System.Net.Http.HttpRequestException: The SSL connection could not be established
```

**Solutions:**

1. Verify certificate is valid:
   ```bash
   # IIS: Right-click site → Edit Bindings → Check cert expiration
   # Linux: openssl x509 -in cert.pem -text -noout | grep -i validity
   ```

2. Bind certificate to site:
   - IIS Manager → Sites → Edit Bindings
   - Select correct certificate

3. Check certificate chain:
   ```bash
   # Ensure intermediate certificates are installed
   ```

---

### "Deployment package is too large"

**Symptoms:**
- Publish folder exceeds 500MB
- Upload times out

**Solutions:**

1. Exclude unnecessary files:
   ```xml
   <!-- In .csproj -->
   <ItemGroup>
	 <Exclude Include="**/node_modules/**" />
	 <Exclude Include="**/dist/**" />
   </ItemGroup>
   ```

2. Use self-contained runtime:
   ```bash
   dotnet publish -c Release --self-contained false
   ```

3. Remove debug symbols:
   ```bash
   dotnet publish -c Release -p:DebugType=none -p:DebugSymbols=false
   ```

---

## Getting Help

### Still Stuck?

1. **Check Existing Issues:**
   - GitHub Issues: https://github.com/Mitko-M/Blog-App/issues
   - GitHub Discussions: https://github.com/Mitko-M/Blog-App/discussions

2. **Provide Information:**
   - Error message (full stack trace)
   - Operating system
   - .NET version
   - Reproduction steps
   - What you've already tried

3. **Open a New Issue:**
   - Use issue template
   - Include error logs
   - Attach screenshots if helpful

4. **Contact Maintainers:**
   - GitHub Issues with `help-wanted` label
   - Discussions board

---

## Quick Reference

| Issue | Quick Fix |
|-------|-----------|
| Port in use | `dotnet run --urls "https://localhost:5002"` |
| SQL Server not found | `SqlLocalDB start mssqllocaldb` |
| NuGet restore fails | `dotnet nuget locals all --clear` |
| Migration not found | `dotnet ef migrations list --project BlogApp.Infrastructure` |
| Build fails | `dotnet clean && dotnet build` |
| Tests fail | `dotnet test --verbosity detailed` |
| View not found | Check view file path matches Controller/Action |
| Login fails | Verify user exists in database |
| Admin access denied | Check user has Admin role |
| Slow performance | Add `.Include()` to queries, enable pagination |

---

**Remember: Always check logs first!** They usually contain the actual error message.
