# Architecture & Project Map

This document summarizes the `blog-app` solution, lists the main responsibilities of each project, and points to the most important folders and types to help new contributors navigate the codebase.

## Solution overview

- `BlogApp.sln` — Visual Studio solution that ties the projects together.

Projects:

- `BlogApp` (web UI)
  - Controllers: `Controllers/` and `Areas/*/Controllers/` implement request handling. Notable controllers: `HomeController`, `PostController`, `ContactController`, `CommentController`, `StatusCodeController` and several admin/user area controllers.
  - Views: Razor views under `Views/` and `Areas/` for UI pages.
  - Components: reusable view components in `Components/` (e.g. `SideWidgetsComponent`, `PaginationComponent`).
  - Static assets: `wwwroot/` holds CSS, JS, images and third-party libs.
  - Extensions: `Extensions/` contains DI and helper extension methods (e.g. `ServiceCollectionExtension`, `ClaimsPrincipalExtensions`).

- `BlogApp.Core` (domain & services)
  - Contracts: `Contracts/` contains service interfaces (e.g. `IPostService`, `IUserService`, `ICommentService`, `IContactService`, `ICategoryService`, `ITagService`, `IAdminService`).
  - Services: `Services/` contains implementations like `PostService`, `UserService`, `CommentService`, `ContactService`, `CategoryService`, `TagService`, `AdminService`.
  - Models: view models, form models and DTOs used by controllers and views (e.g. `PostDetailsViewModel`, `AddPostFormModel`, identity view models).
  - Enumerations & helpers: common enums (e.g. `PostSorting`) and validation constants.

- `BlogApp.Infrastructure` (persistence)
  - `BlogAppDbContext` — EF Core DbContext for the application.
  - Entities: models under `Data/Models/` (e.g. `Post`, `Category`, `Tag`, `Comment`, `ApplicationUser`, `ContactFormEntry`, `PostReport`).
  - Configuration: model configuration classes under `Data/Configuration/` to set up EF mappings.
  - Migrations: EF migrations in `Migrations/` used to create/seed the database.

- `BlogApp.Core.Test`
  - Unit tests for the core service layer (NUnit + Moq). Look for tests such as `PostServiceTests`, `UserServiceTests`, `CommentServiceTest`, `ContactServiceTests`.

## Important components and where to find them

- Service registrations: `BlogApp/Extensions/ServiceCollectionExtension.cs` — registers core services and infrastructure dependencies.
- Entry point: `BlogApp/Program.cs` — application startup and host configuration.
- EF context: `BlogApp.Infrastructure/Data/BlogAppDbContext.cs` — DbSets and OnModelCreating.
- Key service implementations: `BlogApp.Core/Services/PostService.cs`, `UserService.cs`, `CommentService.cs`, `ContactService.cs`, `CategoryService.cs`, `TagService.cs`, `AdminService.cs`.
- Tests: `BlogApp.Core.Test/*` — unit tests that mock repositories/DbContext and validate service behavior.

## How responsibilities are divided

- Controllers (web layer) only orchestrate requests, validate inputs and call into `BlogApp.Core` services.
- Business logic and validation live in `BlogApp.Core` services and models.
- Database mapping, migrations and seed data live in `BlogApp.Infrastructure`.

## Common maintenance tasks

- Update schema: add migration in `BlogApp.Infrastructure` then `dotnet ef database update`.
- Add or modify a service: update the interface in `Contracts/`, implement in `Services/`, then register in `ServiceCollectionExtension`.
- Add UI page: add controller action in `BlogApp/Controllers`, create view under `Views/` and add any components under `Components/`.

## Notes & next steps for documentation

- Consider adding XML documentation comments to public service interfaces and critical methods in `BlogApp.Core` to improve IDE discoverability.
- Add an examples or walkthrough section for common developer tasks (creating a new post flow, writing tests for services).

## Where to look next

- Start at `BlogApp/Program.cs` to understand app startup.
- Browse `BlogApp.Core/Contracts` and `BlogApp.Core/Services` to see the domain API.
- Inspect `BlogApp.Infrastructure/Migrations/` for database history and seeds.

---
This file was generated to accompany an expanded README. If you want, I can also generate a reverse-mapping of controllers → service methods and service → repository usages.
