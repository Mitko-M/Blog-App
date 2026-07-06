# Development Guide

Guide for developers contributing to BlogApp.

## Table of Contents

- [Code Organization](#code-organization)
- [Coding Standards](#coding-standards)
- [Project Structure](#project-structure)
- [Adding Features](#adding-features)
- [Adding Services](#adding-services)
- [Database Changes](#database-changes)
- [Best Practices](#best-practices)
- [Common Tasks](#common-tasks)

---

## Code Organization

BlogApp follows **clean architecture** with clear separation of concerns:

```
BlogApp/                          → Web/Presentation Layer
├── Controllers/                  → Request handlers
├── Views/                        → Razor views and layouts
├── Components/                   → Reusable view components
├── Areas/                        → Admin/User areas
├── Extensions/                   → Helper methods & DI
└── Program.cs                    → App configuration

BlogApp.Core/                     → Business Logic Layer
├── Contracts/                    → Service interfaces
├── Services/                     → Service implementations
├── Models/                       → ViewModels and DTOs
├── Enumerations/                 → Enums
└── Validation/                   → Validation logic

BlogApp.Infrastructure/           → Data Access Layer
├── Data/
│   ├── Models/                   → Database entities
│   ├── Configuration/            → EF configurations
│   ├── Migrations/               → DB migrations
│   └── BlogAppDbContext.cs       → DbContext
├── Common/                       → Constants, utilities
└── Services/                     → Repository patterns (if used)

BlogApp.Core.Test/                → Testing Layer
├── *ServiceTests.cs              → Unit tests
└── Fixtures/                     → Test data
```

---

## Coding Standards

### Naming Conventions

**Classes:**
```csharp
// PascalCase for public classes
public class PostService { }
public class AddPostFormModel { }
```

**Methods:**
```csharp
// PascalCase for public methods, async methods end with Async
public async Task<Post> GetPostAsync(int id) { }
public void ValidatePost(Post post) { }
```

**Variables & Parameters:**
```csharp
// camelCase for local variables and parameters
var postTitle = "Hello";
public void UpdatePost(string postTitle) { }
```

**Constants:**
```csharp
// UPPER_CASE for constants
public const int MaxPostLength = 5000;
public const string DateFormat = "dd MMM yyyy";
```

### Formatting

**Indentation:** 4 spaces (or 1 tab)

**Line Length:** 120 characters recommended

**Braces:** Allman style (opening brace on new line)
```csharp
public class MyClass
{
	public void MyMethod()
	{
		if (condition)
		{
			// code
		}
	}
}
```

**Using Statements:** Group by namespace
```csharp
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Core.Contracts;
```

### Documentation

**Public Methods:**
```csharp
/// <summary>
/// Retrieves all posts matching the specified criteria.
/// </summary>
/// <param name="categoryId">The category to filter by (optional).</param>
/// <param name="pageNumber">The page number (1-based).</param>
/// <returns>A collection of PostViewModel objects.</returns>
public async Task<IEnumerable<PostViewModel>> GetPostsAsync(int? categoryId = null, int pageNumber = 1)
{
	// Implementation
}
```

**Complex Logic:**
```csharp
// Add comments for non-obvious logic
// Check if user has exceeded daily post limit
if (user.PostsToday >= MAX_POSTS_PER_DAY)
{
	throw new InvalidOperationException("Daily post limit reached");
}
```

---

## Project Structure

### BlogApp (Web Layer)

**Controllers:**
- Inherit from `BaseController`
- Use dependency injection for services
- Validate input early
- Return appropriate HTTP status codes

```csharp
public class PostController : BaseController
{
	private readonly IPostService _postService;

	public PostController(IPostService postService)
	{
		_postService = postService;
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> Details(int id)
	{
		var post = await _postService.GetPostById(id);
		if (post == null)
			return NotFound();

		return View(post);
	}
}
```

**Views:**
- Use Razor syntax for dynamic content
- Leverage Bootstrap classes for styling
- Use view components for reusable sections

```html
@model PostDetailsViewModel

<div class="post-details">
	<h1>@Model.Title</h1>
	<p class="metadata">By @Model.AuthorName on @Model.CreatedOn.ToString("dd MMM yyyy")</p>
	<div>@Html.Raw(Model.Content)</div>
</div>
```

**Components:**
- Self-contained, reusable UI sections
- Inherit from `ViewComponent`
- Have their own view in `Views/Shared/Components/`

```csharp
public class SideWidgetsComponent : ViewComponent
{
	private readonly ICategoryService _categoryService;

	public SideWidgetsComponent(ICategoryService categoryService)
	{
		_categoryService = categoryService;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _categoryService.GetCategoriesAsync();
		return View(categories);
	}
}
```

### BlogApp.Core (Business Logic)

**Contracts (Interfaces):**
- Define service methods
- Include XML documentation
- Located in `Contracts/` folder

```csharp
public interface IPostService
{
	/// <summary>
	/// Retrieves all posts with optional filtering.
	/// </summary>
	Task<PostQueryServiceModel> GetAllPostsAsync(
		string? tagName = null,
		string? categoryName = null,
		int currentPage = 1,
		int postsPerPage = 10
	);

	Task AddPostAsync(AddPostFormModel model, string userId);
}
```

**Services:**
- Implement corresponding interface
- Handle business logic and validation
- Use dependency injection
- Catch and handle exceptions

```csharp
public class PostService : IPostService
{
	private readonly BlogAppDbContext _context;

	public PostService(BlogAppDbContext context)
	{
		_context = context;
	}

	public async Task<PostQueryServiceModel> GetAllPostsAsync(string? tagName = null, ...)
	{
		var query = _context.Posts.AsQueryable();

		if (!string.IsNullOrEmpty(tagName))
		{
			query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Name == tagName));
		}

		var posts = await query.ToListAsync();
		return new PostQueryServiceModel { Posts = posts, PostsCount = posts.Count };
	}

	public async Task AddPostAsync(AddPostFormModel model, string userId)
	{
		if (string.IsNullOrWhiteSpace(model.Title))
			throw new ArgumentException("Title is required");

		var post = new Post
		{
			Title = model.Title,
			Content = model.Content,
			UserId = userId,
			CreatedOn = DateTime.Now
		};

		await _context.Posts.AddAsync(post);
		await _context.SaveChangesAsync();
	}
}
```

**Models:**
- ViewModels for views
- DTOs for data transfer
- Form models for POST requests

```csharp
public class AddPostFormModel
{
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public ICollection<PostCategoryFormModel> Categories { get; set; } = new List<PostCategoryFormModel>();
	public ICollection<PostTagFormModel> Tags { get; set; } = new List<PostTagFormModel>();
}
```

### BlogApp.Infrastructure (Persistence)

**Models:**
- Represent database tables
- Include validation attributes
- Use shadow properties where appropriate

```csharp
[Table("Posts")]
public class Post
{
	[Key]
	[Comment("Post identifier")]
	public int Id { get; set; }

	[Required]
	[StringLength(PostTitleMax, MinimumLength = PostTitleMin)]
	[Comment("Post title")]
	public string Title { get; set; } = string.Empty;

	[Required]
	[Comment("Post creation date")]
	public DateTime CreatedOn { get; set; }

	// Navigation properties
	public ApplicationUser? User { get; set; }
	public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}
```

**DbContext Configuration:**
- Fluent API for relationships
- Indexes for performance
- Cascade delete rules

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
	// Configure Post-Comment relationship
	builder.Entity<Comment>()
		.HasOne(c => c.Post)
		.WithMany(p => p.Comments)
		.HasForeignKey(c => c.PostId)
		.OnDelete(DeleteBehavior.Cascade);

	// Index for frequent queries
	builder.Entity<Post>()
		.HasIndex(p => p.UserId);

	builder.Entity<Post>()
		.HasIndex(p => p.CreatedOn);
}
```

---

## Adding Features

### Step-by-Step Process

#### 1. Design the Feature

Define:
- User workflow
- Database changes needed
- Services required
- UI/views needed

**Example:** Adding a "Featured Posts" feature

#### 2. Create/Update Database Models

```csharp
// BlogApp.Infrastructure/Data/Models/Post.cs
public class Post
{
	// ... existing properties

	[Comment("Is this post featured on home page")]
	public bool IsFeatured { get; set; } = false;
}
```

#### 3. Create Migration

```bash
dotnet ef migrations add "AddFeaturedPostsFeature" --project BlogApp.Infrastructure
```

Review generated migration, then apply:

```bash
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

#### 4. Create/Update Services

```csharp
// BlogApp.Core/Contracts/IPostService.cs
public interface IPostService
{
	// ... existing methods

	/// <summary>
	/// Gets featured posts for homepage display.
	/// </summary>
	Task<IEnumerable<PostViewModel>> GetFeaturedPostsAsync(int count = 5);
}

// BlogApp.Core/Services/PostService.cs
public class PostService : IPostService
{
	public async Task<IEnumerable<PostViewModel>> GetFeaturedPostsAsync(int count = 5)
	{
		return await _context.Posts
			.Where(p => p.IsFeatured)
			.OrderByDescending(p => p.CreatedOn)
			.Take(count)
			.ToListAsync();
	}
}
```

#### 5. Create Views/Components

```csharp
// BlogApp/Components/FeaturedPostsComponent.cs
public class FeaturedPostsComponent : ViewComponent
{
	private readonly IPostService _postService;

	public FeaturedPostsComponent(IPostService postService)
	{
		_postService = postService;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var featuredPosts = await _postService.GetFeaturedPostsAsync();
		return View(featuredPosts);
	}
}
```

#### 6. Add Controller Actions

```csharp
// BlogApp/Controllers/PostController.cs
[AllowAnonymous]
public async Task<IActionResult> Featured()
{
	var featuredPosts = await _postService.GetFeaturedPostsAsync();
	return View(featuredPosts);
}

[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> ToggleFeatured(int id)
{
	var post = await _postService.GetPostById(id);
	if (post == null)
		return NotFound();

	post.IsFeatured = !post.IsFeatured;
	await _postService.UpdatePostAsync(post, /* model */);

	return RedirectToAction("Details", new { id });
}
```

#### 7. Write Tests

```csharp
// BlogApp.Core.Test/PostServiceTests.cs
[Test]
public async Task GetFeaturedPostsAsync_ReturnsFeaturedPosts()
{
	// Arrange
	var mockContext = new Mock<BlogAppDbContext>();
	var service = new PostService(mockContext.Object);

	// Act
	var result = await service.GetFeaturedPostsAsync();

	// Assert
	Assert.IsNotEmpty(result);
}
```

#### 8. Update Documentation

- Update [FEATURES.md](./FEATURES.md) with new feature description
- Update [SERVICES.md](./SERVICES.md) with new service methods
- Add XML documentation to code

---

## Adding Services

### Complete Service Example

**1. Define Interface:**

```csharp
// BlogApp.Core/Contracts/INotificationService.cs
public interface INotificationService
{
	Task SendNotificationAsync(int userId, string message);
	Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(int userId);
}
```

**2. Implement Service:**

```csharp
// BlogApp.Core/Services/NotificationService.cs
public class NotificationService : INotificationService
{
	private readonly BlogAppDbContext _context;

	public NotificationService(BlogAppDbContext context)
	{
		_context = context;
	}

	public async Task SendNotificationAsync(int userId, string message)
	{
		var notification = new Notification
		{
			UserId = userId,
			Message = message,
			CreatedOn = DateTime.Now
		};

		await _context.Notifications.AddAsync(notification);
		await _context.SaveChangesAsync();
	}

	public async Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(int userId)
	{
		return await _context.Notifications
			.Where(n => n.UserId == userId)
			.OrderByDescending(n => n.CreatedOn)
			.Select(n => new NotificationViewModel
			{
				Id = n.Id,
				Message = n.Message,
				CreatedOn = n.CreatedOn
			})
			.ToListAsync();
	}
}
```

**3. Register in DI Container:**

```csharp
// BlogApp/Extensions/ServiceCollectionExtension.cs
public static IServiceCollection AddAppServices(this IServiceCollection services)
{
	// ... existing services
	services.AddScoped<INotificationService, NotificationService>();
	return services;
}
```

**4. Use in Controllers:**

```csharp
public class PostController : BaseController
{
	private readonly IPostService _postService;
	private readonly INotificationService _notificationService;

	public PostController(IPostService postService, INotificationService notificationService)
	{
		_postService = postService;
		_notificationService = notificationService;
	}

	[HttpPost]
	public async Task<IActionResult> AddComment(CommentFormModel model)
	{
		await _postService.AddCommentAsync(model);

		// Notify post author
		var post = await _postService.GetPostById(model.PostId);
		await _notificationService.SendNotificationAsync(
			post.User.Id, 
			$"New comment on your post '{post.Title}'"
		);

		return RedirectToAction("Details", new { id = model.PostId });
	}
}
```

---

## Database Changes

### Adding a New Column

```csharp
// 1. Update model
public class Post
{
	// ... existing properties
	public string? MetaDescription { get; set; }
}

// 2. Create migration
dotnet ef migrations add "AddMetaDescriptionToPost" --project BlogApp.Infrastructure

// 3. Review and apply
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

### Adding a New Entity

```csharp
// 1. Create model
public class Review
{
	public int Id { get; set; }
	public int PostId { get; set; }
	public string UserId { get; set; } = string.Empty;
	public int Rating { get; set; } // 1-5 stars
	public string Content { get; set; } = string.Empty;

	public Post? Post { get; set; }
	public ApplicationUser? User { get; set; }
}

// 2. Add to DbContext
public DbSet<Review> Reviews { get; set; }

// 3. Configure relationships
builder.Entity<Review>()
	.HasOne(r => r.Post)
	.WithMany(p => p.Reviews)
	.HasForeignKey(r => r.PostId)
	.OnDelete(DeleteBehavior.Cascade);

// 4. Create migration and apply
dotnet ef migrations add "AddReviewsTable" --project BlogApp.Infrastructure
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

---

## Best Practices

### Async/Await

Always use async methods:
```csharp
// ✓ Good
public async Task<Post?> GetPostAsync(int id)
{
	return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
}

// ✗ Bad
public Post? GetPost(int id)
{
	return _context.Posts.FirstOrDefault(p => p.Id == id);
}
```

### Dependency Injection

Always inject dependencies:
```csharp
// ✓ Good
public class PostService
{
	private readonly BlogAppDbContext _context;

	public PostService(BlogAppDbContext context)
	{
		_context = context;
	}
}

// ✗ Bad
public class PostService
{
	private readonly BlogAppDbContext _context = new BlogAppDbContext();
}
```

### Exception Handling

Handle exceptions appropriately:
```csharp
// ✓ Good
try
{
	await _context.SaveChangesAsync();
}
catch (DbUpdateException ex)
{
	throw new InvalidOperationException("Failed to save post", ex);
}

// ✗ Bad
try
{
	await _context.SaveChangesAsync();
}
catch { } // Silently ignore errors
```

### Input Validation

Validate early:
```csharp
// ✓ Good
public async Task AddPostAsync(AddPostFormModel model, string userId)
{
	if (string.IsNullOrWhiteSpace(model.Title))
		throw new ArgumentException("Title is required");

	// ... rest of implementation
}

// ✗ Bad
public async Task AddPostAsync(AddPostFormModel model, string userId)
{
	// Assume model is valid
	var post = new Post { Title = model.Title };
}
```

### Null Coalescing

Use modern C# features:
```csharp
// ✓ Good
var posts = await _context.Posts.FirstOrDefaultAsync() ?? new List<Post>();
var name = user?.FirstName ?? "Unknown";

// ✗ Bad
var posts = await _context.Posts.FirstOrDefaultAsync();
if (posts == null) posts = new List<Post>();
```

---

## Common Tasks

### Running Tests with Debugging

```bash
# Run tests with detailed output
dotnet test --verbosity detailed

# Run single test
dotnet test --filter TestMethod=GetAllPostsAsync_ShouldReturnPosts
```

### Code Cleanup

```bash
# Format code
dotnet format

# Run analyzers
dotnet build /p:EnforceCodeStyleInBuild=true
```

### Generate API Documentation

```bash
# Generate XML docs
dotnet msbuild /p:DocumentationFile=bin/Release/net10.0/BlogApp.Core.xml
```

### Performance Profiling

Use Visual Studio Diagnostic Tools:
1. Debug → Windows → Performance Profiler
2. Select "CPU Usage"
3. Run application
4. Identify hot paths

---

## See Also

- [TESTING.md](./TESTING.md) - Writing unit tests
- [ARCHITECTURE.md](../ARCHITECTURE.md) - System design
- [SERVICES.md](./SERVICES.md) - Service documentation
