# Features Documentation

Comprehensive guide to all features in BlogApp.

## Table of Contents

- [User Authentication](#user-authentication)
- [Blog Posts](#blog-posts)
- [Comments & Engagement](#comments--engagement)
- [Categories & Tags](#categories--tags)
- [Search & Discovery](#search--discovery)
- [Admin Dashboard](#admin-dashboard)
- [Contact Form](#contact-form)

---

## User Authentication

### Registration

**Flow:**
1. User navigates to `/User/Account/Register`
2. Completes registration form with:
   - First Name (3-50 chars)
   - Last Name (3-50 chars)
   - Username (3-20 chars)
   - Email (10-60 chars)
   - Password (5-20 chars)
3. Form is validated server-side
4. User account is created with "User" role
5. User is redirected to login page

**Validation:**
- All fields required
- Email must be unique
- Username must be unique and contain no spaces
- Password must meet minimum length
- HTML is sanitized to prevent XSS

**Technical Details:**
- Uses ASP.NET Core Identity
- Passwords hashed with PBKDF2
- CSRF protection enabled

---

### Login

**Flow:**
1. User navigates to `/User/Account/Login`
2. Enters username/email and password
3. System verifies credentials against password hash
4. On success:
   - Authentication cookie is set
   - User is redirected to previous page or home
5. On failure:
   - Error message is displayed
   - User remains on login page

**Security:**
- Cookies set as HttpOnly (no JavaScript access)
- Secure flag (HTTPS only in production)
- SameSite=Lax for CSRF protection

---

### Account Management

**Location:** `/User/Account/Manage`

**Available Actions:**
- View profile information
- Update first/last name
- Change password
- View login history

**Permissions:**
- User can only manage their own account
- Admins can manage any user account

---

### Logout

**Location:** `/User/Account/Logout`

**Flow:**
1. User clicks logout
2. Authentication cookie is removed
3. Session ends
4. User redirected to home page

---

### Access Control

**Role-Based:**
- **User Role:** Can create/edit own posts, comment, report posts
- **Admin Role:** Full access to dashboard, user management, moderation

**Authorization:**
- Implemented via `[Authorize]` and `[Authorize(Roles = "Admin")]`
- Area-specific access restrictions in `AdminBaseController` and `UserBaseController`

---

## Blog Posts

### Creating Posts

**Location:** `/Post/Create`

**Requirements:**
- User must be authenticated

**Form Fields:**
- **Title** (5-50 chars)
- **Content** (10-5000 chars) - WYSIWYG editor with TinyMCE
- **Short Description** (10-150 chars) - Preview text
- **Categories** - Multiple selection checkboxes
- **Tags** - Multiple selection checkboxes

**Process:**
1. User fills form
2. Content is sanitized with HtmlSanitizer
3. Categories and tags are associated
4. Post is saved to database with metadata
5. User is redirected to post details
6. Success message displayed

**Technical:**
- Rich text editing via TinyMCE 6.x
- HTML content sanitized to prevent XSS
- Creation and update timestamps recorded
- Post tagged with author's user ID

---

### Viewing Posts

**Home Feed:** `/` or `/Home/All`

**Features:**
- Shows all published posts with pagination
- Post preview displays short description
- Shows post metadata:
  - Author name
  - Publication date
  - Categories and tags
  - Comment count
  - Like/dislike count

**Filtering:**
- By category (dropdown)
- By tag (dropdown)
- By search term (full-text search)

**Sorting:**
- Newest first (default)
- Oldest first
- Most liked/disliked
- Most commented

---

### Post Details

**Location:** `/Post/Details/{id}/{title}`

**View Includes:**
- Full post content
- Author information
- Publication and last updated timestamps
- Categories and tags
- Comment section
- Like/dislike buttons
- Share/report options

**Interactions:**
- Like/dislike post
- Add to favorites
- Post comment
- Report inappropriate content

---

### Editing Posts

**Location:** `/Post/Edit/{id}`

**Requirements:**
- User must be author OR admin
- Returns 403 Forbidden otherwise

**Process:**
1. Form pre-populated with current data
2. User modifies content
3. Categories and tags updated
4. UpdatedOn timestamp changed
5. Post saved
6. User redirected to post details

---

### Deleting Posts

**Location:** `/Post/Delete/{id}`

**Confirmation:**
- Shows post title and content preview
- User must confirm deletion

**Cascade Effects:**
- All comments on post are deleted
- All likes/dislikes removed
- All reports cleared
- Favorites removed

**Permissions:**
- Only post author or admin can delete

---

### My Posts

**Location:** `/Post/Mine`

**Features:**
- Shows only posts by logged-in user
- Same filtering and sorting as home feed
- Action buttons:
  - Edit
  - Delete
  - View

---

## Comments & Engagement

### Adding Comments

**Flow:**
1. User scrolls to comment section on post details page
2. Fills comment form (required field)
3. Submits comment
4. Comment appears immediately with:
   - Author name
   - Creation timestamp
   - Comment text
5. Comment count incremented on post

**Validation:**
- Comment content required
- Length: 1-100 characters
- CSRF token validated

**Technical:**
- Async comment loading via AJAX
- Partial view for comment display

---

### Comment Liking

**Flow:**
1. User hovers over comment
2. Clicks like button
3. Like count increments
4. User's ID stored in CommentLike record
5. Prevents duplicate likes (one per user per comment)

**Dislike:**
- Currently shows dislike count from CommentViewModel
- May expand to implement full dislike functionality

---

### Comment Removal

**Admin Feature:**
- Admins can remove inappropriate comments
- Located in admin dashboard
- Removes comment from database

---

## Categories & Tags

### Browsing by Category

**Flow:**
1. User selects category from sidebar filter
2. Page displays posts with selected category only
3. Category name shown as active filter
4. Can combine with tag filters

**Categories Available:**
- Loaded from database
- Updated via admin panel

---

### Browsing by Tag

**Flow:**
1. User selects tag from sidebar or post details
2. Shows all posts tagged with selected tag
3. Can combine with category filters

---

### Tag Management

**Admin Only:**
- Add new tags
- Edit existing tags
- Delete tags (if no associated posts)

---

## Search & Discovery

### Full-Text Search

**Location:** Search bar in navigation

**Features:**
- Searches post titles and content
- Returns matching posts with pagination
- Highlights match context
- Can combine with category/tag filters

**Implementation:**
- LINQ/EF Core query with `.Contains()`
- Can be enhanced to SQL Full-Text Search

---

### Filtering

**Available Filters:**
- **Category** - Sidebar dropdown
- **Tag** - Sidebar or inline
- **Search Term** - Search bar
- **Combine Filters** - All filters work together

**Pagination:**
- Default: 10 posts per page
- Configurable via query string
- Next/previous navigation

---

### Sorting Options

**Available Sort Orders:**
- Newest First (default)
- Oldest First
- Most Liked
- Most Disliked
- Most Commented

**Implementation:**
- PostSorting enum in BlogApp.Core
- Applied in PostService.GetAllPostsAsync()

---

## Admin Dashboard

### Access

**Location:** `/Admin/Home/Index`

**Requirements:**
- User must have Admin role
- Returns 403 Forbidden otherwise

### Admin Home

**Dashboard Views:**
- Total users count
- Total admins count
- Total posts count
- Recent activity overview

---

### User Management

**Location:** `/Admin/User/Manage?userName={username}`

**View:**
- User profile information
- Posts by user
- Comments by user
- Warning history

**Actions:**
- Issue warning
- Ban user
- View user activity

---

### Warnings System

**Flow:**
1. Admin views post report
2. Clicks "Warn User" button
3. System creates Warning record linked to user
4. User receives notification (future feature)
5. Warning count incremented for user

**Consequences:**
- Visible in user profile (admin view)
- Multiple warnings may lead to ban
- Warnings timestamped for tracking

---

### User Banning

**Process:**
1. Admin navigates to user management
2. Clicks "Ban User"
3. User.Banned flag set to true
4. User cannot login
5. Posts remain but marked as from banned user

**Unbanning:**
- Admin can unban user
- Restores login ability

---

### Post Reports

**Location:** `/Admin/Report/All`

**Features:**
- Lists all post reports
- Shows report reason
- Displays reported post preview
- Shows report submitter

**Actions:**
- View full report details
- Warn post author
- Dismiss report
- Delete reported post

---

### Contact Form Management

**Location:** `/Admin/Contact/All`

**View:**
- All contact form submissions
- Sortable by date, name, subject
- Pagination support

**Actions:**
- View full message
- Delete submission
- Reply (via email - future feature)

---

### Admin Reports

**Location:** `/Admin/Report/`

**Features:**
- View flagged posts
- Review report reasons
- Take moderation actions

---

## Contact Form

### Submitting Contact Form

**Location:** `/Contact`

**Form Fields:**
- **Name** (2-50 chars)
- **Subject** (2-50 chars)
- **Message** (5-1000 chars)
- **Email** (valid email format)

**Process:**
1. User fills form (anonymous submission allowed)
2. Form submitted
3. Submission stored in database
4. Confirmation page shown
5. Admin notified (via email - future feature)

**Validation:**
- All fields required
- Email format validated
- Length constraints enforced
- CSRF protection enabled

**Technical:**
- Stored in ContactFormEntry table
- CreatedOn timestamp recorded
- Can be viewed/managed in admin panel

---

## Engagement Metrics

### Like/Dislike Posts

**Features:**
- Users can like or dislike posts
- One per user per post
- Count displayed on post cards
- Togglable (click again to unlike)

**Data:**
- LikeDislike table tracks:
  - PostId
  - UserId
  - IsLike (true=like, false=dislike)
- Unique constraint prevents duplicates

---

### Like Comments

**Features:**
- Users can like comments
- Count displayed on each comment
- One per user per comment

---

### Favorites

**Features:**
- Users can bookmark posts
- My Favorites view (future feature)
- Tracks in Favorites table

---

## Recommended Content

### Sidebar Widgets

**Available Widgets:**
- Recent Posts
- Popular Tags
- Popular Categories
- Top Authors (future)

**Technical:**
- SideWidgetsComponent displays data
- Cached for performance

---

## Security Features

### Content Sanitization

- All user-submitted HTML sanitized with HtmlSanitizer
- Prevents XSS attacks
- Preserves formatting

### CSRF Protection

- Anti-forgery tokens on all forms
- AutoValidateAntiforgeryTokenAttribute on actions

### Authentication

- Secure cookies (HttpOnly, Secure flag)
- Password hashing with PBKDF2
- Session management

### Authorization

- Role-based access control
- Resource-based authorization checks
- Ownership validation (users can only edit own posts)

---

## Accessibility

### Features for Users with Disabilities

- Semantic HTML markup
- ARIA labels on interactive elements
- Color contrast compliant
- Keyboard navigation support
- Alt text on images

---

## Performance Features

### Caching

- View component output caching (future)
- Database query optimization
- Pagination to limit data transfer

### Pagination

- Limits results per page
- Reduces memory usage
- Improves page load times

---

## Future Features

Planned enhancements:
- Real-time notifications (SignalR)
- Email notifications for comments
- Email digest subscriptions
- Post scheduling for future publication
- Draft posts
- Post revisions/history
- Advanced analytics
- Social sharing buttons
- API for third-party integrations
- Multi-language support
- Dark mode theme

---

## See Also

- [DEVELOPMENT.md](./DEVELOPMENT.md) - Adding new features
- [TESTING.md](./TESTING.md) - Testing features
- [SERVICES.md](./SERVICES.md) - Service methods for features
