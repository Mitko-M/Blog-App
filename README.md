# BlogApp

BlogApp is an ASP.NET MVC blogging platform implementing post creation, comments, tagging, categories, admin actions and a small reporting/workflow system. It is structured as a solution with three main projects: the web UI (`BlogApp`), the domain/core (`BlogApp.Core`) and the data layer (`BlogApp.Infrastructure`).

## Quick summary

- Web UI: Razor views, components and controllers in the `BlogApp` project.
- Domain & services: business logic, DTOs and service contracts in `BlogApp.Core`.
- Persistence: Entity Framework Core models, configurations and `Migrations/` in `BlogApp.Infrastructure`.
- Tests: unit tests for core services in `BlogApp.Core.Test`.

## Features

- User authentication (register/login/manage account)
- CRUD for posts with rich text editor
- Comments with moderation and likes/dislikes
- Categories and tags for filtering and discovery
- Search and sorting support
- Admin dashboard for user, contact and report management
- Contact form with storage and admin preview
- Unit tests for core services (NUnit + Moq)

## Prerequisites

- .NET SDK (6.0+ / matching the solution target)
- SQL Server (localdb or a networked instance)
- (Optional) `dotnet-ef` tools to run migrations

## Run locally

1. Clone the repository: https://github.com/Mitko-M/Blog-App.git
2. From the solution root, restore packages:

```bash
dotnet restore
```

3. Update the connection string in `BlogApp/appsettings.json` (or `appsettings.Development.json`) to point to your SQL Server instance.
4. Apply EF migrations (optional if database already seeded):

```bash
dotnet tool install --global dotnet-ef # if you don't have dotnet-ef
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

5. Run the application from the solution or using CLI from the `BlogApp` folder:

```bash
dotnet run --project BlogApp
```

6. Run unit tests from the solution root:

```bash
dotnet test
```

## Project structure (high level)

- `BlogApp/` — ASP.NET MVC web project (controllers, views, components, wwwroot)
- `BlogApp.Core/` — service interfaces (`Contracts/`), implementations (`Services/`), view models (`Models/`) and enums
- `BlogApp.Infrastructure/` — EF Core `BlogAppDbContext`, entity models, configurations and `Migrations/`
- `BlogApp.Core.Test/` — unit tests for core services

For a detailed architecture and file mapping see `ARCHITECTURE.md`.

## Tests

This repository uses NUnit and Moq for unit testing. Tests live in `BlogApp.Core.Test` and cover the core service layer.

Run all tests:

```bash
dotnet test
```

## Contributing

1. Fork the repo and create a feature branch.
2. Add tests for your feature where appropriate.
3. Open a pull request with a clear description of changes.

## License

This project is licensed under the MIT License — see `LICENSE.txt` for details.

## Contact

If you want help or to report issues, open an issue in the repository.

---
_This README was expanded automatically by a workspace documentation pass. See `ARCHITECTURE.md` for more details._
