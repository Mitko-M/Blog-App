# Controller Documentation Guidelines

This document outlines the controller structure and documentation patterns for BlogApp.

## Controller Architecture

### Main Controllers (Root)

#### BaseController
- **Purpose:** Base class for all main controllers providing common functionality
- **Location:** `BlogApp/Controllers/BaseController.cs`
- **Responsibilities:** Common authorization logic, helper methods

#### HomeController
- **Purpose:** Homepage and public post listing
- **Location:** `BlogApp/Controllers/HomeController.cs`
- **Key Actions:**
  - `All(AllPostsQueryModel model)` - GET - Display all posts with filtering/sorting
  - `Error()` - GET - Error page display
- **Access:** Anonymous users allowed

#### PostController
- **Purpose:** Post CRUD operations and public post interaction
- **Location:** `BlogApp/Controllers/PostController.cs`
- **Key Actions:**
  - `Create()` - GET/POST - Create new blog post (authenticated)
  - `Edit(int id)` - GET/POST - Edit existing post (owner only)
  - `Delete(int id)` - GET/POST - Delete post (owner/admin)
  - `Details(int id, int page)` - GET - View post with comments (anonymous)
  - `Mine(int currentPage)` - GET - View user's own posts (authenticated)
  - `Report(int postId)` - GET/POST - Report post for violation (authenticated)
- **Access:** Mixed (public for reading, authenticated for creating)

#### CommentController
- **Purpose:** Comment operations on posts
- **Location:** `BlogApp/Controllers/CommentController.cs`
- **Key Actions:**
  - `Create()` - POST - Add comment to post (authenticated)
  - `LikeComment()` - POST - Like a comment (authenticated)
- **Access:** Authenticated users only

#### SortAndFilterController
- **Purpose:** AJAX endpoints for dynamic sorting/filtering
- **Location:** `BlogApp/Controllers/SortAndFilterController.cs`
- **Key Actions:**
  - Various filter and sort actions (AJAX)
- **Access:** Authenticated users

#### ContactController
- **Purpose:** Contact form submission
- **Location:** `BlogApp/Controllers/ContactController.cs`
- **Key Actions:**
  - `Index()` - GET/POST - Submit contact form
- **Access:** Authenticated users

#### StatusCodeController
- **Purpose:** Handle HTTP status codes (404, 403, etc.)
- **Location:** `BlogApp/Controllers/StatusCodeController.cs`
- **Access:** Automatic error routing

### Admin Area Controllers

#### Admin/HomeController
- **Purpose:** Admin dashboard
- **Location:** `BlogApp/Areas/Admin/Controllers/HomeController.cs`
- **Key Actions:**
  - `Index()` - GET - Admin dashboard overview (admin only)
  - `Admins()` - GET - List admin users (admin only)
  - `Users()` - GET - List all users (admin only)
- **Access:** Admin role required

#### Admin/UserController
- **Purpose:** User management (banning, warnings)
- **Location:** `BlogApp/Areas/Admin/Controllers/UserController.cs`
- **Key Actions:**
  - `Manage(string userId)` - GET/POST - User admin panel
  - Ban/unban user operations
- **Access:** Admin role required

#### Admin/ReportController
- **Purpose:** Post report review and moderation
- **Location:** `BlogApp/Areas/Admin/Controllers/ReportController.cs`
- **Key Actions:**
  - `All(int page)` - GET - List all post reports
  - `Preview(int reportId)` - GET - View report details
- **Access:** Admin role required

#### Admin/RegisterController
- **Purpose:** Register new admin users
- **Location:** `BlogApp/Areas/Admin/Controllers/RegisterController.cs`
- **Key Actions:**
  - `Index()` - GET/POST - Admin registration
- **Access:** Admin role required

#### Admin/BannController
- **Purpose:** User banning workflow
- **Location:** `BlogApp/Areas/Admin/Controllers/BannController.cs`
- **Key Actions:**
  - `Ban(string userId)` - POST - Ban user from platform
  - `Unban(string userId)` - POST - Lift user ban
- **Access:** Admin role required

#### Admin/ContactController
- **Purpose:** Contact form entry management
- **Location:** `BlogApp/Areas/Admin/Controllers/ContactController.cs`
- **Key Actions:**
  - `All(int page)` - GET - List contact forms
  - `Preview(int id)` - GET - View contact form details
- **Access:** Admin role required

### User Area Controllers

#### User/AccountController
- **Purpose:** User account management (login, register, profile)
- **Location:** `BlogApp/Areas/User/Controllers/AccountController.cs`
- **Key Actions:**
  - `Register()` - GET/POST - User registration
  - `Login()` - GET/POST - User login
  - `Logout()` - POST - User logout
  - `Manage()` - GET/POST - Edit user profile
- **Access:** Anonymous for register/login, authenticated for manage

#### User/AccessController
- **Purpose:** Access denied/unauthorized pages
- **Location:** `BlogApp/Areas/User/Controllers/AccessController.cs`
- **Key Actions:**
  - `Index()` - GET - Display access denied message
- **Access:** Anonymous

#### User/UserBaseController
- **Purpose:** Base class for User area controllers
- **Location:** `BlogApp/Areas/User/Controllers/UserBaseController.cs`
- **Responsibilities:** User area common logic

## Documentation Standards for Controllers

### Class-Level Documentation

Every controller class should have XML documentation explaining:
- **Purpose:** What feature/area this controller handles
- **Key Responsibilities:** Main business functions
- **Authentication:** Required roles or authorization
- **Example:**
```csharp
/// <summary>
/// Manages blog post creation, editing, and deletion.
/// 
/// Provides endpoints for users to:
/// - Create new blog posts with rich text editor
/// - Edit existing posts (owner only)
/// - Delete posts (owner or admin)
/// - View post details with comments
/// 
/// Requires authentication for create/edit/delete operations.
/// </summary>
public class PostController : BaseController
```

### Action-Level Documentation

Every public action should have XML documentation explaining:
- **HTTP Method and Route:** GET, POST, etc.
- **Purpose:** What the action does
- **Parameters:** Input parameters and their meaning
- **Authorization:** Required roles or conditions
- **Returns:** What view or redirect is returned
- **Example:**
```csharp
/// <summary>
/// GET: /Post/Create
/// Displays the create post form.
/// 
/// Returns: Create view with PostCreateModel template
/// Requires: Authenticated user
/// </summary>
[Authorize]
[HttpGet]
public IActionResult Create()
```

## Common Patterns

### Parameter Documentation
```csharp
/// <summary>
/// GET: /Post/Details/{id}
/// 
/// Parameters:
/// - id: Post ID to display
/// - page: Comment page number (default: 1)
/// 
/// Returns: Post details view with comments
/// </summary>
public async Task<IActionResult> Details(int id, int page = 1)
```

### Async Actions
```csharp
/// <summary>
/// POST: /Post/Create
/// Saves new blog post to database.
/// 
/// Returns: Redirect to post details if successful, Create view if validation fails
/// Throws: Returns BadRequest if model is invalid
/// </summary>
[HttpPost]
public async Task<IActionResult> Create(PostCreateModel model)
```

### Authorization Notes
```csharp
/// <summary>
/// POST: /Admin/User/Ban
/// Bans a user from the platform (admin only).
/// 
/// Returns: Redirect to user list
/// Requires: Admin role
/// Throws: Unauthorized if not admin
/// </summary>
[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> Ban(string userId)
```

## Adding Controllers

When adding a new controller:

1. **Create the class** inheriting from BaseController
2. **Add class-level XML documentation** with purpose and responsibilities
3. **Add action-level XML documentation** for each public action
4. **Include route/HTTP method** in documentation
5. **Document parameters** with their purposes
6. **Include authorization** requirements in docs
7. **Note return values** and possible outcomes

## Testing Controllers

See [TESTING.md](./TESTING.md) for controller testing examples.

## Related Documentation

- [FEATURES.md](./FEATURES.md) - Feature workflows and their corresponding controller actions
- [SERVICES.md](./SERVICES.md) - Service layer that controllers depend on
- [DEVELOPMENT.md](./DEVELOPMENT.md) - Development guidelines and patterns
