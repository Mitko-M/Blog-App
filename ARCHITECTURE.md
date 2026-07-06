# Architecture & Project Map

This document summarizes the `blog-app` solution, names the architecture pattern used, lists the main responsibilities of each project, and points to the most important folders and types to help new contributors navigate the codebase.

## Architecture pattern

The solution follows a layered architecture (Presentation → Application/Service → Infrastructure/Data). Responsibilities are separated as:

- Presentation layer: `BlogApp` — controllers, Razor views, view components and static assets. Handles HTTP concerns and user interactions.
- Application/Service layer: `BlogApp.Core` — service interfaces and implementations that encapsulate business logic and orchestrate data access.
- Infrastructure/Data layer: `BlogApp.Infrastructure` — EF Core entities, `BlogAppDbContext`, model configurations and migrations.

This separation keeps controllers thin and focuses business rules inside services, which makes unit testing and maintenance easier.

## Backend services (detailed)

The `BlogApp.Core/Services` folder contains concrete service classes that implement the application logic. Below is a concise reference for each major service and its public surface area.

### PostService (`BlogApp.Core/Services/PostService.cs`)

- Purpose: manage posts lifecycle (create, read, update, delete), provide query models for listing and details, and handle post reports.
- Key public methods:
  - `Task AddPostAsync(AddPostFormModel model, string userId)` — creates a `Post`, associates selected categories/tags and persists changes.
  - `Task<AddPostFormModel> GetPostFormModel()` — prepares an empty post form model populated with selectable categories and tags.
  - `List<int> RequestSelectionToList(string values)` — helper to parse comma-separated id lists into integers.
  - `Task<Post?> GetPostById(int id)` — fetches a post with related categories, tags, user, comments, favorites and likes/dislikes.
  - `Task<PostQueryServiceModel> GetAllPostsAsync(...)` — returns paged, filtered and sorted posts for public listing.
  - `Task<PostQueryServiceModel> GetMinePostsAsync(string UserId, ...)` — similar to above but scoped to a single user's posts.
  - `Task<AddPostFormModel> GetPostToEditAsync(int id, Post givenPost = null)` — prepares a populated edit form for a post (supports test injection via `givenPost`).
  - `Task UpdatePostAsync(Post post, AddPostFormModel model)` — updates post fields and synchronizes categories/tags relations.
  - `Task DeletePostAsync(Post post)` — removes related likes, favorites, comments, reports and the post itself.
  - `PostDetailsViewModel GetPostDetailsViewModel(Post post)` — builds a view model used by the UI's post details page.
  - `Task ReportPost(PostReportViewModel reportViewModel, Post givenPost = null)` — adds a `PostReport` entry to a post.

### UserService (`BlogApp.Core/Services/UserService.cs`)

- Purpose: retrieve and update user profile data, and check user state (e.g. banned).
- Key public methods:
  - `Task<ApplicationUserWithAllDataViewModel> GetUserById(string userId)` — returns a view model with user data, role and basic post summary.
  - `Task<bool> IsUserBanned(string userId)` — checks whether the user is marked as banned.
  - `Task UpdateUserData(ApplicationUserViewModel model, string userId)` — updates editable user fields and normalizes username/email.

### CommentService (`BlogApp.Core/Services/CommentService.cs`)

- Purpose: add/like/unlike comments and load comments for a post.
- Key public methods:
  - `Task AddCommentAsync(CommentFormModel model)` — validates post existence and inserts a `Comment`.
  - `Task LikeComment(int commentId, string userId)` — inserts a `CommentLike` for a comment.
  - `Task<IEnumerable<CommentViewModel>> LoadCommentsAsync(int postId)` — loads ordered comment view models including likes.
  - `Task UnlikeComment(int commentId, string userId)` — removes an existing `CommentLike`.

### ContactService (`BlogApp.Core/Services/ContactService.cs`)

- Purpose: persist contact form entries submitted by users.
- Key public methods:
  - `Task SubmitContactForm(ContactViewModel model)` — creates a `ContactFormEntry` and saves it.

### CategoryService (`BlogApp.Core/Services/CategoryService.cs`)

- Purpose: provide category lists for UI and selection in post forms.
- Key public methods:
  - `Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()` — returns all categories.
  - `Task<IEnumerable<PostCategoryFormModel>> GetCategoriesWithIsSelected()` — returns selectable category models used in forms.

### TagService (`BlogApp.Core/Services/TagService.cs`)

- Purpose: provide tag lists for UI and selection in post forms.
- Key public methods:
  - `Task<IEnumerable<TagViewModel>> GetTagsAsync()` — returns all tags.
  - `Task<IEnumerable<PostTagFormModel>> GetTagsWithIsSelected()` — returns selectable tag models used in forms.

### AdminService (`BlogApp.Core/Services/AdminService.cs`)

- Purpose: admin operations — manage users, reports and contact form entries.
- Key public methods:
  - `Task Bann(string userName)` / `Task UnBann(string userName)` — set or clear the `Banned` flag for a user.
  - `Task DeleteContactFormEntry(int id)` — removes a contact form entry.
  - `Task DeleteReport(int id)` — removes a post report.
  - `Task<IEnumerable<ApplicationUserViewModel>> GetAdminsAsync()` / `GetUsersAsync()` / `GetAllUsersAsync()` — list users by role.
  - `Task<IEnumerable<ContactAdminViewModel>> GetAllContactFormsAsync()` — list stored contact entries for admin review.
  - `Task<IEnumerable<PostReportsAdminViewModel>> GetAllReportsAsync()` — list all post reports for moderation.
  - `Task<ContactAdminViewModel> GetContactFormById(int id)` and `Task<PostReportsAdminViewModel> GetReportById(int id)` — fetch single admin view models.
  - `Task WarnApplicationUser(int reportId, int postId, string userId)` — hide a post, create a `Warning` and ban the user after 3 warnings.

## How services interact with persistence

- Services use `BlogAppDbContext` (EF Core) directly to query and save entities. Some services (e.g. `PostService`) call other services (`ICategoryService`, `ITagService`, `IUserService`) to obtain supporting data or to keep responsibilities separated.
- The pattern favors service-centric business logic rather than fat controllers or repository abstractions — the services are the application layer.

## Next documentation options

- Generate a cross-reference mapping: controller actions → service calls → entities used.
- Create a developer quickstart for common tasks (add feature, add migration, write a unit test for a service).
- Add XML comments to `BlogApp.Core/Contracts` interfaces so IDE tooltips explain intent.

If you want, I can generate the controllers → services map next (it will produce a comprehensive list of controller actions and the exact service methods they call).

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
