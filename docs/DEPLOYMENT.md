# Deployment Guide

Guide for deploying BlogApp to production environments.

## Table of Contents

- [Pre-Deployment Checklist](#pre-deployment-checklist)
- [Environment Configuration](#environment-configuration)
- [Database Deployment](#database-deployment)
- [Deployment Options](#deployment-options)
  - [Windows Server](#windows-server-iis)
  - [Linux/Docker](#linuxdocker)
  - [Azure App Service](#azure-app-service)
- [Security Hardening](#security-hardening)
- [Performance Optimization](#performance-optimization)
- [Monitoring & Logging](#monitoring--logging)
- [Rollback Procedures](#rollback-procedures)

---

## Pre-Deployment Checklist

- [ ] All unit tests passing
- [ ] Code reviewed by maintainer
- [ ] Database migrations reviewed
- [ ] Security vulnerabilities scanned
- [ ] Performance tested under load
- [ ] Staging deployment successful
- [ ] Documentation updated
- [ ] Deployment plan communicated to team
- [ ] Rollback procedure documented
- [ ] Backup strategy in place

---

## Environment Configuration

### Configuration by Environment

**Development:**
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogAppDb;Trusted_Connection=true;"
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Debug"
	}
  }
}
```

**Staging:**
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=staging-sql-server;Database=BlogAppStaging;User Id=sa;Password=***;"
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Information"
	}
  }
}
```

**Production:**
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=prod-sql-server;Database=BlogAppProd;Encrypt=true;User Id=***;Password=***;"
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Warning",
	  "Microsoft.AspNetCore": "Error"
	}
  }
}
```

### Environment Variables

Set via system or container environment:

```bash
# Production environment
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80;https://+:443
DOTNET_ENVIRONMENT=Production
DefaultConnection=<connection-string>
```

### Secrets Management

**Never commit secrets!** Use one of these approaches:

**Option 1: User Secrets (Development only)**
```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=..."
```

**Option 2: Environment Variables**
```bash
# Linux/macOS
export DefaultConnection="server=..."

# Windows
set DefaultConnection="server=..."
```

**Option 3: Secure Vaults**
- Azure Key Vault (recommended for Azure)
- AWS Secrets Manager (for AWS)
- HashiCorp Vault (on-premises)

**Option 4: Docker Secrets**
```bash
echo "connection-string" | docker secret create db_connection -
```

---

## Database Deployment

### Pre-Deployment Database Checks

```bash
# 1. Backup existing database
# SQL Server Management Studio → Right-click DB → Tasks → Backup

# 2. Verify migrations
dotnet ef migrations list --project BlogApp.Infrastructure

# 3. Test migration on staging
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp --environment Staging
```

### Migration Deployment

**Option 1: Automatic (Recommended for small deployments)**

Add to Program.cs:
```csharp
// Auto-apply migrations on startup
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<BlogAppDbContext>();
	dbContext.Database.Migrate();
}

await app.RunAsync();
```

**Option 2: Manual (Recommended for large deployments)**

```bash
# Apply migrations separately before deploying app
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp --environment Production
```

### Backup Strategy

**Before Each Deployment:**

```bash
# SQL Server backup
BACKUP DATABASE [BlogAppProd] 
TO DISK = N'C:\Backups\BlogAppProd_$(date).bak'
WITH INIT, COMPRESSION, STATS = 10;
```

**Restore from Backup:**

```bash
RESTORE DATABASE [BlogAppProd] 
FROM DISK = N'C:\Backups\BlogAppProd_2024.bak'
WITH REPLACE, RECOVERY;
```

---

## Deployment Options

### Windows Server + IIS

#### Prerequisites

- Windows Server 2019 or later
- IIS 10.0+
- .NET Hosting Bundle (includes ASP.NET Core runtime)
- SQL Server (local or remote)

#### Installation Steps

1. **Download Hosting Bundle**
   - Visit https://dotnet.microsoft.com/download/dotnet/10.0
   - Download "Hosting Bundle" for your OS
   - Run installer: `dotnet-hosting-10.0.x-win.exe`
   - Restart IIS: `iisreset`

2. **Publish Application**

```bash
# Publish to folder
dotnet publish -c Release -o C:\apps\BlogApp
```

3. **Create IIS Site**

- Open IIS Manager
- Right-click "Sites" → "Add Website"
- Configure:
  - Site name: BlogApp
  - Physical path: `C:\apps\BlogApp`
  - Binding: `http://yourdomain.com:80` or `https://yourdomain.com:443`

4. **Configure Application Pool**

- Right-click Application Pool → Edit
- .NET CLR version: No Managed Code
- Managed Pipeline Mode: Integrated

5. **SSL Certificate**

```bash
# Using Let's Encrypt (certbot)
certbot certonly --manual --preferred-challenges dns -d yourdomain.com

# Import in IIS:
# 1. Export PFX file
# 2. Right-click "Server Certificates" → "Import"
# 3. Select binding → assign certificate
```

6. **Verify Deployment**

- Navigate to `https://yourdomain.com`
- Check application is running
- Monitor IIS logs: `C:\inetpub\logs\LogFiles\`

#### Health Check Endpoint

```csharp
// Add to Program.cs
app.MapHealthChecks("/health");

// Monitor at https://yourdomain.com/health
```

---

### Linux/Docker

#### Docker Deployment

**Dockerfile:**

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["BlogApp/BlogApp.csproj", "BlogApp/"]
COPY ["BlogApp.Core/BlogApp.Core.csproj", "BlogApp.Core/"]
COPY ["BlogApp.Infrastructure/BlogApp.Infrastructure.csproj", "BlogApp.Infrastructure/"]
RUN dotnet restore "BlogApp/BlogApp.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80 443
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "BlogApp.dll"]
```

**docker-compose.yml:**

```yaml
version: '3.8'

services:
  blogapp:
	build: .
	ports:
	  - "80:80"
	  - "443:443"
	environment:
	  ASPNETCORE_ENVIRONMENT: Production
	  DefaultConnection: "Server=sqlserver;Database=BlogAppDb;User Id=sa;Password=MySecurePassword123;"
	depends_on:
	  - sqlserver
	restart: unless-stopped

  sqlserver:
	image: mcr.microsoft.com/mssql/server:2022-latest
	environment:
	  ACCEPT_EULA: Y
	  MSSQL_SA_PASSWORD: MySecurePassword123!
	volumes:
	  - sqlserver_data:/var/opt/mssql
	ports:
	  - "1433:1433"
	restart: unless-stopped

volumes:
  sqlserver_data:
```

**Deploy:**

```bash
# Build and start containers
docker-compose up -d

# View logs
docker-compose logs -f blogapp

# Scale replicas (with load balancer)
docker-compose up -d --scale blogapp=3
```

#### Linux Service (Systemd)

Create `/etc/systemd/system/blogapp.service`:

```ini
[Unit]
Description=BlogApp ASP.NET Core Application
After=network.target

[Service]
Type=notify
User=blogapp
WorkingDirectory=/home/blogapp/BlogApp
ExecStart=/usr/bin/dotnet /home/blogapp/BlogApp/BlogApp.dll
Restart=on-failure
RestartSec=10
Environment="ASPNETCORE_ENVIRONMENT=Production"
Environment="ASPNETCORE_URLS=http://localhost:5000"

[Install]
WantedBy=multi-user.target
```

**Manage Service:**

```bash
# Enable autostart
sudo systemctl enable blogapp

# Start service
sudo systemctl start blogapp

# View logs
sudo journalctl -u blogapp -f

# Restart
sudo systemctl restart blogapp
```

---

### Azure App Service

#### Prerequisites

- Azure subscription
- Azure CLI installed
- SQL Azure database

#### Deployment Steps

1. **Create Resource Group**

```bash
az group create --name BlogApp-RG --location eastus
```

2. **Create App Service Plan**

```bash
az appservice plan create \
  --name BlogApp-Plan \
  --resource-group BlogApp-RG \
  --sku B2 \
  --is-linux
```

3. **Create SQL Database**

```bash
az sql server create \
  --name blogapp-sql \
  --resource-group BlogApp-RG \
  --admin-user sqladmin \
  --admin-password MySecurePassword123!

az sql db create \
  --resource-group BlogApp-RG \
  --server blogapp-sql \
  --name BlogAppDb
```

4. **Create App Service**

```bash
az webapp create \
  --resource-group BlogApp-RG \
  --plan BlogApp-Plan \
  --name blogapp \
  --runtime "DOTNET|10.0"
```

5. **Deploy Application**

```bash
# Using GitHub Actions (recommended)
# Copy workflow from .github/workflows/

# Or manual deployment
git clone https://github.com/Mitko-M/Blog-App.git
cd blog-app
az webapp up --name blogapp --resource-group BlogApp-RG
```

6. **Configure Environment Variables**

```bash
az webapp config appsettings set \
  --resource-group BlogApp-RG \
  --name blogapp \
  --settings \
	ASPNETCORE_ENVIRONMENT=Production \
	DefaultConnection="Server=tcp:blogapp-sql.database.windows.net,1433;Initial Catalog=BlogAppDb;Persist Security Info=False;User ID=sqladmin;Password=***;Encrypt=True;Connection Timeout=30;"
```

7. **Enable SSL/TLS**

```bash
# Add custom domain
az webapp config ssl bind \
  --resource-group BlogApp-RG \
  --name blogapp \
  --certificate-name mydomaincert \
  --ssl-type SNI
```

#### GitHub Actions Deployment

**.github/workflows/deploy.yml:**

```yaml
name: Deploy to Azure

on:
  push:
	branches: [main]

jobs:
  build-and-deploy:
	runs-on: ubuntu-latest
	steps:
	  - uses: actions/checkout@v2

	  - name: Setup .NET
		uses: actions/setup-dotnet@v1
		with:
		  dotnet-version: '10.0.x'

	  - name: Build
		run: dotnet build --configuration Release

	  - name: Test
		run: dotnet test

	  - name: Publish
		run: dotnet publish -c Release -o ./publish

	  - name: Deploy to Azure
		uses: azure/webapps-deploy@v2
		with:
		  app-name: blogapp
		  publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
		  package: ./publish
```

---

## Security Hardening

### HTTPS/TLS

**Enable HTTPS Redirect:**

```csharp
// In Program.cs
app.UseHttpsRedirection();
app.UseHsts(); // Strict-Transport-Security header
```

**HTTP Security Headers:**

```csharp
app.Use(async (context, next) =>
{
	context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
	context.Response.Headers.Add("X-Frame-Options", "DENY");
	context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
	context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
	await next();
});
```

### Input Validation & Sanitization

- All user input is validated server-side
- HTML content is sanitized with HtmlSanitizer
- CSRF tokens on all forms
- SQL injection prevented by EF Core

### Authentication & Authorization

```csharp
// Cookie security
services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;      // No JavaScript access
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
	options.Cookie.SameSite = SameSiteMode.Strict;           // CSRF prevention
});
```

### Secrets & Credentials

- Never commit `appsettings.Production.json`
- Use environment variables or vaults
- Rotate credentials regularly
- Use service principals (Azure)

### Database Security

```sql
-- Create least-privilege user
CREATE LOGIN [blogapp_user] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [blogapp_user] FOR LOGIN [blogapp_user];

-- Grant minimal permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON BlogAppDb TO [blogapp_user];
GRANT EXECUTE ON ALL OBJECTS TO [blogapp_user];

-- Encrypt connection
-- Connection string: Encrypt=true;TrustServerCertificate=false;
```

### Firewall & Network

- Enable Windows Firewall (or equivalent)
- Restrict database access by IP
- Use VPNs for admin access
- Enable DDoS protection (cloud)

---

## Performance Optimization

### Caching

```csharp
// Response caching
services.AddResponseCaching();
app.UseResponseCaching();

[ResponseCache(Duration = 3600)] // 1 hour
public async Task<IActionResult> Index()
{
	// ...
}
```

### Database Optimization

```csharp
// Query optimization
var posts = await _context.Posts
	.Include(p => p.User)
	.Include(p => p.Comments)
	.AsNoTracking() // Read-only
	.Skip((page - 1) * pageSize)
	.Take(pageSize)
	.ToListAsync();
```

### Static File Compression

```csharp
services.AddResponseCompression(options =>
{
	options.EnableForHttps = true;
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes
		.Concat(new[] { "application/json" });
});

app.UseResponseCompression();
```

### CDN Usage

Serve static assets from CDN:

```html
<link href="https://cdn.example.com/css/site.css" rel="stylesheet" />
<script src="https://cdn.example.com/js/site.js"></script>
```

---

## Monitoring & Logging

### Application Insights (Azure)

```csharp
// In Program.cs
services.AddApplicationInsightsTelemetry();

// Log custom metrics
var client = new TelemetryClient();
client.TrackEvent("PostCreated");
client.TrackMetric("PostCount", posts.Count());
```

### Logging Configuration

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning",
	  "Microsoft.EntityFrameworkCore": "Information"
	}
  }
}
```

### Health Checks

```csharp
app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = WriteResponse
});

services.AddHealthChecks()
	.AddDbContextCheck<BlogAppDbContext>()
	.AddCheck("DatabaseConnection", async () =>
	{
		try
		{
			await _context.Database.ExecuteSqlAsync($"SELECT 1");
			return HealthCheckResult.Healthy();
		}
		catch
		{
			return HealthCheckResult.Unhealthy();
		}
	});
```

### Log Aggregation

- Use ELK Stack (Elasticsearch, Logstash, Kibana)
- Or use cloud solutions (Azure Monitor, AWS CloudWatch)
- Aggregate logs from multiple instances

---

## Rollback Procedures

### Database Rollback

**If migrations fail:**

```bash
# Identify last good migration
dotnet ef migrations list --project BlogApp.Infrastructure

# Rollback to previous migration
dotnet ef database update "PreviousMigrationName" \
  --project BlogApp.Infrastructure \
  --startup-project BlogApp
```

### Application Rollback

**IIS/Windows:**

```bash
# Keep previous version in C:\apps\BlogApp.Previous
# Stop current site
iisreset /stop

# Restore previous version
robocopy C:\apps\BlogApp.Previous C:\apps\BlogApp /MIR

# Start IIS
iisreset /start
```

**Docker:**

```bash
# Rollback to previous image
docker-compose down
docker rmi blogapp:latest
docker pull blogapp:v1.0.0
docker tag blogapp:v1.0.0 blogapp:latest
docker-compose up -d
```

**Azure App Service:**

```bash
# View deployment history
az webapp deployment list --resource-group BlogApp-RG --name blogapp

# Swap slots
az webapp deployment slot swap \
  --resource-group BlogApp-RG \
  --name blogapp \
  --slot staging
```

---

## Disaster Recovery

### Backup Strategy

- Daily database backups (full + differential)
- Weekly off-site backups
- Test restore procedures monthly
- Document RTO/RPO requirements

### Incident Response

1. **Detect:** Monitor health checks, logs, user reports
2. **Assess:** Determine scope and impact
3. **Communicate:** Notify stakeholders
4. **Remediate:** Apply fix or rollback
5. **Verify:** Test fix in staging
6. **Deploy:** Promote to production
7. **Document:** Post-incident review

---

## See Also

- [SETUP.md](./docs/SETUP.md) - Local development setup
- [DATABASE.md](./docs/DATABASE.md) - Database information
- [TROUBLESHOOTING.md](./docs/TROUBLESHOOTING.md) - Common issues

---

**Safe Deployments! 🚀**
