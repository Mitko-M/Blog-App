# Services Reference

This document describes all service interfaces and their methods in the BlogApp.Core layer. These services provide the core business logic for the blogging platform.

## Table of Contents

- [IPostService](#ipostservice)
- [ICommentService](#icommentservice)
- [ICategoryService](#icategoryservice)
- [ITagService](#itagservice)
- [IUserService](#iuserservice)
- [IAdminService](#iadminservice)
- [IContactService](#icontactservice)
- [Common Patterns](#common-patterns)

---

## IPostService

**Location:** `BlogApp.Core/Contracts/IPostService.cs`

Handles all post-related operations including CRUD, search, filtering, and reporting.

### Methods

#### GetAllPostsAsync
```csharp
Task<PostQueryServiceModel> GetAllPostsAsync(
	string? tagName = null,
	string? categoryName = null,
	PostSorting sorting = PostSorting.None,
	int currentPage = 1,
	int postsPerPage = 1,
	string searchTerm = null
);
```

**Purpose:** Retrieve all published posts with filtering and pagination.

**Parameters:**
- `tagName` - Filter by specific tag (optional)
- `categoryName` - Filter by specific category (optional)
- `sorting` - Sort order (None, NewestFirst, OldestFirst, MostLiked, etc.)
- `currentPage` - Page number (1-based indexing)
- `postsPerPage` - Number of posts per page
- `searchTerm` - Full-text search term (optional)

**Returns:** `PostQueryServiceModel` containing posts and total count

**Example:**
```csharp
var result = await _postService.GetAllPostsAsync(
	tagName: "C#",
	categoryName: "Programming",
	sorting: PostSorting.NewestFirst,
	currentPage: 1,
	postsPerPage: 10
);
```

#### GetMinePostsAsync
```csharp
Task<PostQueryServiceModel> GetMinePostsAsync(
	string userId,
	string? tagName = null,
	string? categoryName = null,
	PostSorting sorting = PostSorting.None,
	int currentPage = 1,
	int postsPerPage = 1,
	string searchTerm = null
);
```

**Purpose:** Retrieve posts authored by a specific user.

**Parameters:** Same as `GetAllPostsAsync` plus `userId` identifying the author.

**Returns:** `PostQueryServiceModel` with user's posts only

#### AddPostAsync
```csharp
Task AddPostAsync(AddPostFormModel postModel, string userId);
```

**Purpose:** Create a new blog post.

**Parameters:**
- `postModel` - Form model containing title, content, categories, tags
- `userId` - ID of the post author

**Throws:** `ArgumentException` if post creation fails

**Example:**
```csharp
var model = new AddPostFormModel
{
	Title = "Getting Started with C#",
	Content = "<p>Rich HTML content...</p>",
	ShortDescription = "An introduction to C#",
	Categories = categoryList,
	Tags = tagList
};
await _postService.AddPostAsync(model, userId);
```

#### GetPostById
```csharp
Task<Post?> GetPostById(int id);
```

**Purpose:** Fetch a post entity by ID.

**Parameters:** `id` - Post identifier

**Returns:** Post entity or null if not found

#### GetPostToEditAsync
```csharp
Task<AddPostFormModel> GetPostToEditAsync(int id, Post givenPost = null);
```

**Purpose:** Retrieve post data in edit form format.

**Parameters:**
- `id` - Post identifier
- `givenPost` - Optional: pre-loaded Post entity (avoids duplicate DB query)

**Returns:** `AddPostFormModel` populated with post data

#### UpdatePostAsync
```csharp
Task UpdatePostAsync(Post post, AddPostFormModel model);
```

**Purpose:** Update an existing post.

**Parameters:**
- `post` - The Post entity to update
- `model` - New data from form submission

**Throws:** `ArgumentException` on validation failure

#### DeletePostAsync
```csharp
Task DeletePostAsync(Post post);
```

**Purpose:** Delete a post and all related data.

**Parameters:** `post` - Post entity to delete

**Note:** Cascades to delete comments, likes, reports associated with post

#### GetPostFormModel
```csharp
Task<AddPostFormModel> GetPostFormModel();
```

**Purpose:** Create empty form model pre-populated with categories and tags.

**Returns:** `AddPostFormModel` ready for use in create view

#### GetPostDetailsViewModel
```csharp
PostDetailsViewModel GetPostDetailsViewModel(Post post);
```

**Purpose:** Convert Post entity to details view model.

**Parameters:** `post` - Post entity

**Returns:** `PostDetailsViewModel` with formatted data for display

---

## ICommentService

**Location:** `BlogApp.Core/Contracts/ICommentService.cs`

Manages comments on posts including creation, moderation, and engagement.

### Methods

#### AddCommentAsync
```csharp
Task AddCommentAsync(CommentFormModel model);
```

**Purpose:** Add a new comment to a post.

**Parameters:** `model` - Contains UserId, PostId, and Content

**Throws:** `ArgumentException` if comment creation fails

#### LoadCommentsAsync
```csharp
Task<IEnumerable<CommentViewModel>> LoadCommentsAsync(int postId);
```

**Purpose:** Retrieve all comments for a post.

**Parameters:** `postId` - Post identifier

**Returns:** Collection of `CommentViewModel`

#### GetCommentByIdAsync
```csharp
Task<Comment?> GetCommentByIdAsync(int commentId);
```

**Purpose:** Get a specific comment by ID.

**Parameters:** `commentId` - Comment identifier

**Returns:** Comment entity or null

#### RemoveCommentAsync
```csharp
Task RemoveCommentAsync(int commentId);
```

**Purpose:** Delete a comment (admin moderation).

**Parameters:** `commentId` - Comment to delete

#### LikeCommentAsync
```csharp
Task LikeCommentAsync(int commentId, string userId);
```

**Purpose:** User likes a comment.

**Parameters:**
- `commentId` - Comment identifier
- `userId` - User performing the action

#### UnlikeCommentAsync
```csharp
Task UnlikeCommentAsync(int commentId, string userId);
```

**Purpose:** User removes their like from a comment.

**Parameters:** Same as LikeCommentAsync

---

## ICategoryService

**Location:** `BlogApp.Core/Contracts/ICategoryService.cs`

Manages post categories for content organization.

### Methods

#### GetCategoriesAsync
```csharp
Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();
```

**Purpose:** Retrieve all categories.

**Returns:** Collection of `CategoryViewModel`

#### GetCategoryByNameAsync
```csharp
Task<Category?> GetCategoryByNameAsync(string name);
```

**Purpose:** Get a category by name.

**Parameters:** `name` - Category name

**Returns:** Category entity or null

#### AddCategoryAsync
```csharp
Task AddCategoryAsync(string name);
```

**Purpose:** Create a new category (admin only).

**Parameters:** `name` - Category name

---

## ITagService

**Location:** `BlogApp.Core/Contracts/ITagService.cs`

Manages post tags for fine-grained categorization.

### Methods

#### GetTagsAsync
```csharp
Task<IEnumerable<TagViewModel>> GetTagsAsync();
```

**Purpose:** Retrieve all tags.

**Returns:** Collection of `TagViewModel`

#### GetTagByNameAsync
```csharp
Task<Tag?> GetTagByNameAsync(string name);
```

**Purpose:** Get a tag by name.

**Parameters:** `name` - Tag name

**Returns:** Tag entity or null

#### AddTagAsync
```csharp
Task AddTagAsync(string name);
```

**Purpose:** Create a new tag (admin only).

**Parameters:** `name` - Tag name

---

## IUserService

**Location:** `BlogApp.Core/Contracts/IUserService.cs`

Handles user-related operations.

### Methods

#### GetUserByIdAsync
```csharp
Task<ApplicationUserViewModel?> GetUserByIdAsync(string userId);
```

**Purpose:** Retrieve user details.

**Parameters:** `userId` - User identifier

**Returns:** `ApplicationUserViewModel` or null

#### UpdateUserAsync
```csharp
Task UpdateUserAsync(ApplicationUser user);
```

**Purpose:** Update user profile.

**Parameters:** `user` - User entity with updated data

---

## IAdminService

**Location:** `BlogApp.Core/Contracts/IAdminService.cs`

Administrative operations including user management, reports, and moderation.

### Methods

#### GetAdminsAsync
```csharp
Task<IEnumerable<ApplicationUserViewModel>> GetAdminsAsync();
```

**Purpose:** Retrieve all admin users.

**Returns:** Collection of admin view models

#### GetUsersAsync
```csharp
Task<IEnumerable<ApplicationUserViewModel>> GetUsersAsync();
```

**Purpose:** Retrieve all regular users.

**Returns:** Collection of user view models

#### GetAllReportsAsync
```csharp
Task<IEnumerable<PostReportsAdminViewModel>> GetAllReportsAsync();
```

**Purpose:** Retrieve all user reports on posts.

**Returns:** Collection of report view models

#### GetAllContactFormEntriesAsync
```csharp
Task<IEnumerable<ContactAdminViewModel>> GetAllContactFormEntriesAsync();
```

**Purpose:** Retrieve all contact form submissions.

**Returns:** Collection of contact form entries

#### WarnApplicationUser
```csharp
Task WarnApplicationUser(int reportId, int postId, string postOwnerId);
```

**Purpose:** Issue a warning to a user (triggered by post report).

**Parameters:**
- `reportId` - Report identifier
- `postId` - Reported post
- `postOwnerId` - User ID to warn

#### BannApplicationUser
```csharp
Task BannApplicationUser(string userId);
```

**Purpose:** Ban a user account.

**Parameters:** `userId` - User to ban

#### UnbannApplicationUser
```csharp
Task UnbannApplicationUser(string userId);
```

**Purpose:** Remove ban from user.

**Parameters:** `userId` - User to unban

#### ManageUserByUserName
```csharp
Task<ApplicationUserWithAllDataViewModel> ManageUserByUserName(string userName);
```

**Purpose:** Get complete user data for admin management.

**Parameters:** `userName` - User's username

**Returns:** Complete user details including posts, comments, warnings

---

## IContactService

**Location:** `BlogApp.Core/Contracts/IContactService.cs`

Manages contact form submissions.

### Methods

#### SendContactFormAsync
```csharp
Task SendContactFormAsync(ContactViewModel model);
```

**Purpose:** Process and store contact form submission.

**Parameters:** `model` - Contact form data (name, subject, message)

**Throws:** `ArgumentException` if validation fails

#### GetContactFormEntryByIdAsync
```csharp
Task<ContactFormEntry?> GetContactFormEntryByIdAsync(int id);
```

**Purpose:** Retrieve a specific contact entry.

**Parameters:** `id` - Entry identifier

**Returns:** Contact form entry or null

#### DeleteContactFormEntryAsync
```csharp
Task DeleteContactFormEntryAsync(int id);
```

**Purpose:** Delete a contact form entry.

**Parameters:** `id` - Entry to delete

---

## Common Patterns

### Error Handling

All service methods follow this pattern:

```csharp
try
{
	// Operation
	await _context.SaveChangesAsync();
}
catch (Exception ex)
{
	throw new ArgumentException("Operation failed: " + message, ex);
}
```

**Handling errors in controllers:**

```csharp
try
{
	await _service.AddPostAsync(model, userId);
}
catch (ArgumentException ex)
{
	return BadRequest(ex.Message);
}
```

### Asynchronous Operations

All service methods are async (`async`/`await`):

```csharp
// Always await service calls
var posts = await _postService.GetAllPostsAsync();

// In async controllers
public async Task<IActionResult> Index()
{
	var posts = await _postService.GetAllPostsAsync();
	return View(posts);
}
```

### Null Safety

Services return `null` for "not found" scenarios:

```csharp
var post = await _postService.GetPostById(id);
if (post == null)
{
	return NotFound();
}
```

### View Models vs Entities

- **Entities** (`Post`, `Comment`, etc.) - Database models
- **ViewModels** (`PostDetailsViewModel`, etc.) - Display data for views

Services typically return ViewModels to controllers:

```csharp
// Returns entity
Post entity = await _postService.GetPostById(1);

// Returns ViewModel
PostDetailsViewModel viewModel = _postService.GetPostDetailsViewModel(entity);
```

### Dependency Injection

Services are registered in `Program.cs` and injected into controllers:

```csharp
public class PostController : Controller
{
	private readonly IPostService _postService;

	public PostController(IPostService postService)
	{
		_postService = postService;
	}
}
```

---

## Service Registration

All services are registered in `BlogApp/Extensions/ServiceCollectionExtension.cs`:

```csharp
public static IServiceCollection AddAppServices(this IServiceCollection services)
{
	services.AddScoped<IPostService, PostService>();
	services.AddScoped<ICommentService, CommentService>();
	services.AddScoped<ICategoryService, CategoryService>();
	services.AddScoped<ITagService, TagService>();
	services.AddScoped<IUserService, UserService>();
	services.AddScoped<IAdminService, AdminService>();
	services.AddScoped<IContactService, ContactService>();
	// ...
}
```

All services use **Scoped** lifetime (new instance per request).

---

## See Also

- [DEVELOPMENT.md](./DEVELOPMENT.md) - Adding new services
- [TESTING.md](./TESTING.md) - Testing services
- [ARCHITECTURE.md](../ARCHITECTURE.md) - Service layer architecture
