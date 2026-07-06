# Setup & Installation Guide

This guide will help you set up BlogApp for development or deployment.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Development Setup](#development-setup)
- [Database Configuration](#database-configuration)
- [Running the Application](#running-the-application)
- [Database Migrations](#database-migrations)
- [Testing Setup](#testing-setup)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required

- **.NET 10 SDK** (Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0))
  - Verify installation: `dotnet --version`
- **SQL Server** (one of the following):
  - SQL Server LocalDB (included with Visual Studio)
  - SQL Server Express (free edition)
  - SQL Server Standard or Enterprise
- **Git** for repository management
- **Visual Studio 2022** or **Visual Studio Code** with C# extension (recommended)

### Optional

- **SQL Server Management Studio (SSMS)** for database administration
- **Postman** or **Insomnia** for API testing (when REST API is added)
- **Docker** for containerized development environment

---

## Development Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/Mitko-M/Blog-App.git
cd blog-app
```

### Step 2: Restore NuGet Packages

```bash
dotnet restore
```

This downloads all required NuGet packages defined in the project files.

### Step 3: Create User Secrets (Development)

User secrets store sensitive configuration locally without committing to version control:

```bash
cd BlogApp
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

### Step 4: Configure appsettings

Edit `BlogApp/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogAppDb;Trusted_Connection=true;"
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning",
	  "Microsoft.EntityFrameworkCore": "Information"
	}
  },
  "AllowedHosts": "*"
}
```

---

## Database Configuration

### SQL Server LocalDB

**Connection String:**
```
Server=(localdb)\mssqllocaldb;Database=BlogAppDb;Trusted_Connection=true;
```

**To verify LocalDB is installed:**
```bash
SqlLocalDB info
```

**To create a new LocalDB instance:**
```bash
SqlLocalDB create "BlogAppInstance"
SqlLocalDB start "BlogAppInstance"
```

### SQL Server Express

**Connection String:**
```
Server=YOUR_COMPUTER_NAME\SQLEXPRESS;Database=BlogAppDb;Trusted_Connection=true;
```

Replace `YOUR_COMPUTER_NAME` with your machine name.

### Remote SQL Server

**Connection String:**
```
Server=your-server-address;Database=BlogAppDb;User Id=sa;Password=your-password;
```

**With Azure SQL Database:**
```
Server=tcp:your-server.database.windows.net,1433;Initial Catalog=BlogAppDb;Persist Security Info=False;User ID=your-user;Password=your-password;Encrypt=True;Connection Timeout=30;
```

### SQLite (Testing/Demo)

Update the connection in `BlogApp/Program.cs` to use SQLite for development:

```csharp
// For SQLite
services.AddDbContext<BlogAppDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// For SQL Server (default)
services.AddDbContext<BlogAppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## Running the Application

### From Visual Studio

1. Set `BlogApp` as the startup project
2. Press `F5` or click "Run" button
3. The application launches at `https://localhost:5001`

### From Command Line

```bash
cd BlogApp
dotnet run
```

Or from solution root:

```bash
dotnet run --project BlogApp
```

### Accessing the Application

- **Home Page:** `https://localhost:5001/`
- **Login:** `https://localhost:5001/User/Account/Login`
- **Admin Panel:** `https://localhost:5001/Admin/Home/Index` (requires Admin role)

### Default Credentials

After initial migration, a default admin account is seeded. Check the migration files for specific credentials.

---

## Database Migrations

### Install Entity Framework Core Tools (One Time)

```bash
dotnet tool install --global dotnet-ef
```

### Apply Migrations

```bash
# From solution root
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

### Create a New Migration

When you modify database models:

```bash
dotnet ef migrations add "YourMigrationName" --project BlogApp.Infrastructure --startup-project BlogApp
```

### View Pending Migrations

```bash
dotnet ef migrations list --project BlogApp.Infrastructure --startup-project BlogApp
```

### Revert to Previous Migration

```bash
# Revert one migration
dotnet ef database update "PreviousMigrationName" --project BlogApp.Infrastructure --startup-project BlogApp
```

### Remove Latest Migration (If Not Applied)

```bash
dotnet ef migrations remove --project BlogApp.Infrastructure --startup-project BlogApp
```

---

## Testing Setup

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Project

```bash
dotnet test BlogApp.Core.Test
```

### Run with Coverage

```bash
dotnet test /p:CollectCoverage=true
```

### Run Specific Test Class

```bash
dotnet test --filter TestClass=BlogApp.Core.Test.PostServiceTests
```

### Run Specific Test Method

```bash
dotnet test --filter TestClass=BlogApp.Core.Test.PostServiceTests&TestMethod=GetAllPostsAsync_ReturnsCorrectPosts_WhenNoFiltersApplied
```

---

## Project Structure Quick Reference

```
blog-app/
├── BlogApp/                          # Web application (UI)
│   ├── Controllers/                  # HTTP request handlers
│   ├── Views/                        # Razor views and layouts
│   ├── Components/                   # Reusable view components
│   ├── Areas/                        # Admin and User areas
│   ├── wwwroot/                      # Static files (CSS, JS, images)
│   ├── Extensions/                   # Extension methods and DI
│   ├── Program.cs                    # Application entry point
│   ├── appsettings.json              # Configuration
│   └── BlogApp.csproj
│
├── BlogApp.Core/                     # Business logic layer
│   ├── Contracts/                    # Service interfaces
│   ├── Services/                     # Service implementations
│   ├── Models/                       # View models and DTOs
│   ├── Enumerations/                 # Enums
│   └── BlogApp.Core.csproj
│
├── BlogApp.Infrastructure/           # Persistence layer
│   ├── Data/
│   │   ├── Models/                   # Database entity models
│   │   ├── Configuration/            # EF model configurations
│   │   ├── Migrations/               # Database migrations
│   │   └── BlogAppDbContext.cs       # EF DbContext
│   ├── Common/                       # Shared constants and utilities
│   └── BlogApp.Infrastructure.csproj
│
├── BlogApp.Core.Test/                # Unit tests
│   ├── *ServiceTests.cs              # Service test classes
│   └── BlogApp.Core.Test.csproj
│
└── BlogApp.sln                       # Solution file
```

---

## Environment Configuration

### Development Environment

Set in `.env` or system environment variables:

```bash
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:5001
```

### Production Environment

For deployment:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
```

---

## Troubleshooting

### "Cannot find SQL Server"

**Solution:**
- Verify SQL Server is running: `SqlLocalDB info`
- Check connection string spelling
- Ensure database credentials are correct

### "Migrations not found"

**Solution:**
```bash
dotnet ef migrations list --project BlogApp.Infrastructure
```

### "Package restore fails"

**Solution:**
```bash
dotnet nuget locals all --clear
dotnet restore
```

### "Port 5001 is already in use"

**Solution:**
```bash
dotnet run --project BlogApp --urls "https://localhost:5002"
```

### "Entity Framework Core tools not installed"

**Solution:**
```bash
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

### "Cannot login with admin account"

**Solution:**
1. Check if migrations have been applied: `dotnet ef migrations list`
2. Apply migrations: `dotnet ef database update`
3. Verify seed data in migration files

### Tests Fail After Code Changes

**Solution:**
```bash
# Rebuild and run tests
dotnet clean
dotnet build
dotnet test --no-build
```

---

## Next Steps

1. **Learn the Architecture:** Read [ARCHITECTURE.md](./ARCHITECTURE.md)
2. **Understand the Features:** Read [FEATURES.md](./FEATURES.md)
3. **Start Development:** Read [DEVELOPMENT.md](./DEVELOPMENT.md)
4. **Write Tests:** Read [TESTING.md](./TESTING.md)

---

**Need help?** Check [TROUBLESHOOTING.md](./TROUBLESHOOTING.md) or open an issue on GitHub.
