# Database Schema & Design

This document describes the BlogApp database schema, entity relationships, and data model.

## Table of Contents

- [Overview](#overview)
- [Entity Relationship Diagram](#entity-relationship-diagram)
- [Core Entities](#core-entities)
- [Authentication Entities](#authentication-entities)
- [Validation Constants](#validation-constants)
- [Migrations](#migrations)
- [Data Seeding](#data-seeding)

---

## Overview

BlogApp uses **Entity Framework Core** with **SQL Server** as the primary database. The database is designed following best practices:

- **Normalized schema** to minimize data redundancy
- **Foreign key relationships** to maintain referential integrity
- **Indexes** on frequently queried columns
- **Soft delete patterns** where appropriate
- **Audit columns** (CreatedOn, UpdatedOn) for tracking changes

---

## Entity Relationship Diagram

```
┌──────────────────────┐
│  ApplicationUser     │
│  (Identity Users)    │
└──────────────────────┘
		 │
		 ├─── 1:N ────→ Post
		 │               (posts by user)
		 │
		 ├─── 1:N ────→ Comment
		 │               (comments by user)
		 │
		 ├─── 1:N ────→ ContactFormEntry
		 │
		 └─── 1:N ────→ Warning
						 (warnings issued to user)

	   ┌────────────┐
	   │    Post    │
	   └────────────┘
		 │
		 ├─── 1:N ────→ Comment
		 │
		 ├─── 1:N ────→ PostReport
		 │
		 ├─── M:N ────→ Category (via PostCategory)
		 │
		 ├─── M:N ────→ Tag (via PostTag)
		 │
		 ├─── 1:N ────→ LikeDislike
		 │
		 └─── 1:N ────→ Favorite

	   ┌─────────────┐
	   │   Comment   │
	   └─────────────┘
		 │
		 ├─── 1:N ────→ CommentLike
		 │
		 └─── 0:1 ────→ Post
```

---

## Core Entities

### Post

**Table Name:** `Posts`

Represents a blog post.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Post identifier |
| Title | NVARCHAR(50) | NOT NULL | Post title (5-50 chars) |
| Content | NVARCHAR(5000) | NOT NULL | Post body in HTML (10-5000 chars) |
| ShortDescription | NVARCHAR(150) | NOT NULL | Preview text (10-150 chars) |
| CreatedOn | DATETIME2 | NOT NULL | Creation timestamp |
| UpdatedOn | DATETIME2 | NOT NULL | Last update timestamp |
| UserId | NVARCHAR(450) | FK, NOT NULL | Author (references AspNetUsers.Id) |

**Relationships:**
- One-to-Many with Comment (cascade delete)
- Many-to-Many with Category (via PostCategory)
- Many-to-Many with Tag (via PostTag)
- One-to-Many with PostReport
- One-to-Many with LikeDislike
- One-to-Many with Favorite

**Indexes:**
- Clustered: Id
- Non-clustered: UserId, CreatedOn

---

### Comment

**Table Name:** `Comments`

Represents user comments on posts.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Comment identifier |
| Content | NVARCHAR(100) | NOT NULL | Comment text (1-100 chars) |
| CreatedOn | DATETIME2 | NOT NULL | Creation timestamp |
| UpdatedOn | DATETIME2 | NOT NULL | Last update timestamp |
| PostId | INT | FK, NOT NULL | Referenced post (cascade delete) |
| UserId | NVARCHAR(450) | FK, NOT NULL | Comment author |

**Relationships:**
- Many-to-One with Post (cascade delete, no action)
- One-to-Many with CommentLike

**Indexes:**
- Clustered: Id
- Non-clustered: PostId, UserId

---

### Category

**Table Name:** `Categories`

Post categories for organization.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Category identifier |
| Name | NVARCHAR(40) | NOT NULL, Unique | Category name (3-40 chars) |

**Relationships:**
- Many-to-Many with Post (via PostCategory)

---

### Tag

**Table Name:** `Tags`

Post tags for fine-grained categorization.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Tag identifier |
| Name | NVARCHAR(30) | NOT NULL, Unique | Tag name (3-30 chars) |

**Relationships:**
- Many-to-Many with Post (via PostTag)

---

### PostCategory (Junction Table)

**Table Name:** `PostsCategories`

Associates posts with categories (many-to-many relationship).

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| PostId | INT | PK FK | Post identifier |
| CategoryId | INT | PK FK | Category identifier |

**Composite Primary Key:** (PostId, CategoryId)

---

### PostTag (Junction Table)

**Table Name:** `PostsTags`

Associates posts with tags (many-to-many relationship).

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| PostId | INT | PK FK | Post identifier |
| TagId | INT | PK FK | Tag identifier |

**Composite Primary Key:** (PostId, TagId)

---

### LikeDislike

**Table Name:** `LikesDislikes`

Tracks user likes/dislikes on posts.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Record identifier |
| PostId | INT | FK, NOT NULL | Post (cascade delete) |
| UserId | NVARCHAR(450) | FK, NOT NULL | User who liked/disliked |
| IsLike | BIT | NOT NULL | True=Like, False=Dislike |

**Constraints:**
- Unique constraint: (PostId, UserId) - one user can only like/dislike once per post

---

### CommentLike

**Table Name:** `CommentsLikes`

Tracks user likes on comments.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Record identifier |
| CommentId | INT | FK, NOT NULL | Comment (cascade delete) |
| UserId | NVARCHAR(450) | FK, NOT NULL | User who liked |

**Constraints:**
- Unique constraint: (CommentId, UserId)

---

### Favorite

**Table Name:** `Favorites`

User bookmarks for posts.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Record identifier |
| PostId | INT | FK, NOT NULL | Post (cascade delete) |
| UserId | NVARCHAR(450) | FK, NOT NULL | User who favorited |

---

### PostReport

**Table Name:** `PostsReports`

User reports for post moderation.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Report identifier |
| PostId | INT | FK, NOT NULL | Reported post (cascade delete) |
| UserId | NVARCHAR(450) | FK, NOT NULL | User who reported |
| Content | NVARCHAR(1000) | NOT NULL | Report reason (5-1000 chars) |
| CreatedOn | DATETIME2 | NOT NULL | Report timestamp |
| Status | INT | Default 0 | Report status (0=Pending, 1=Reviewed, 2=Dismissed) |

---

### ContactFormEntry

**Table Name:** `ContactFormEntries`

Contact form submissions.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Entry identifier |
| Name | NVARCHAR(50) | NOT NULL | Sender name (2-50 chars) |
| Subject | NVARCHAR(50) | NOT NULL | Subject (2-50 chars) |
| Message | NVARCHAR(1000) | NOT NULL | Message (5-1000 chars) |
| Email | NVARCHAR(60) | NOT NULL | Sender email |
| CreatedOn | DATETIME2 | NOT NULL | Submission timestamp |

---

## Authentication Entities

### ApplicationUser

**Table Name:** `AspNetUsers` (inherited from Identity)

Extends ASP.NET Identity IdentityUser.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | NVARCHAR(450) | PK | User identifier |
| UserName | NVARCHAR(256) | Unique, NOT NULL | Login username (3-20 chars) |
| Email | NVARCHAR(256) | Unique, NOT NULL | Email (10-60 chars) |
| PasswordHash | NVARCHAR(MAX) | NOT NULL | Hashed password |
| FirstName | NVARCHAR(50) | NOT NULL | User's first name (3-50 chars) |
| LastName | NVARCHAR(50) | NOT NULL | User's last name (3-50 chars) |
| Banned | BIT | Default 0 | Account ban status |
| EmailConfirmed | BIT | | Email verification status |
| PhoneNumber | NVARCHAR(MAX) | | Phone number |
| TwoFactorEnabled | BIT | | 2FA status |

**Relationships:**
- One-to-Many with Post
- One-to-Many with Comment
- One-to-Many with ContactFormEntry
- One-to-Many with Warning

---

### Warning

**Table Name:** `Warnings`

User warning records for moderation.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, Identity | Warning identifier |
| Reason | NVARCHAR(MAX) | NOT NULL | Warning reason |
| IssuedOn | DATETIME2 | NOT NULL | Timestamp |
| UserId | NVARCHAR(450) | FK, NOT NULL | User receiving warning |

---

### Identity Role Entities

**Tables:**
- `AspNetRoles` - Role definitions
- `AspNetUserRoles` - User-to-role mappings (junction table)

**Standard Roles:**
- `Admin` - Administrator access
- `User` - Regular user access

---

## Validation Constants

All text field lengths are enforced via `BlogApp.Infrastructure.Common.ValidationConstants`:

### Post Constraints
- Title: 5-50 characters
- Content: 10-5000 characters
- ShortDescription: 10-150 characters

### User Constraints
- FirstName: 3-50 characters
- LastName: 3-50 characters
- UserName: 3-20 characters
- Email: 10-60 characters
- Password: 5-20 characters

### Content Constraints
- Category Name: 3-40 characters
- Tag Name: 3-30 characters
- Comment Content: 1-100 characters
- Contact Form Name: 2-50 characters
- Contact Form Subject: 2-50 characters
- Contact Form Message: 5-1000 characters
- Post Report Content: 5-1000 characters

---

## Migrations

### Viewing Migration History

```bash
dotnet ef migrations list --project BlogApp.Infrastructure
```

**Current Migrations:**
1. `InitialMigration` - Core schema setup
2. `SeedingDataWithAdmin` - Admin user seeding
3. `AddedPostReports` - PostReport table
4. `AddedWarningTable` - Warning table
5. `AddedBannedPropertyInApplicationUserModel` - Banned column
6. `AddedContactFormEntryModel` - Contact form table
7. `AddedCreatedOnColumnInContactFormEntry` - Audit columns

### Creating New Migrations

When modifying models:

```bash
# 1. Modify entity models in BlogApp.Infrastructure/Data/Models/

# 2. Create migration
dotnet ef migrations add "DescriptiveNameHere" `
  --project BlogApp.Infrastructure `
  --startup-project BlogApp

# 3. Review generated migration in Migrations folder

# 4. Apply to database
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

### Migration Best Practices

1. **Descriptive Names:** Use clear, past-tense names
   - ✓ `AddedUserBannedColumn`
   - ✗ `Migration1`, `Update`

2. **One Logical Change Per Migration:** Avoid mixing unrelated changes

3. **Test Locally First:** Apply and test migrations in development before pushing

4. **Never Modify Applied Migrations:** Create new migration to fix mistakes

5. **Provide Rollback Path:** Document how to revert if needed

---

## Data Seeding

### Initial Seed Data

Located in migration files:

**AdminConfiguration.cs** - Admin user seeding
- Default admin account created during migration
- Pre-configured roles

**CategoryConfiguration.cs & TagConfiguration.cs** - Initial categories and tags
- Loaded from JSON configuration files
- Located in `Data/Configuration/`

### Adding Seed Data

```csharp
// In migration Up() method
modelBuilder.Entity<Category>().HasData(
	new Category { Id = 1, Name = "Programming" },
	new Category { Id = 2, Name = "Design" }
);

// In OnModelCreating
modelBuilder.Entity<Category>().HasData(
	new { Id = 1, Name = "Programming" },
	new { Id = 2, Name = "Design" }
);
```

### JSON Seed Files

Categories and tags can be configured via JSON:

**blogapp-Infrastructure/Data/Configuration/categoryconfig.json**
**BlogApp.Infrastructure/Data/Configuration/tagconfig.json**

Format:
```json
[
  { "id": 1, "name": "Programming" },
  { "id": 2, "name": "Design" }
]
```

---

## Database Maintenance

### Backup Database

```bash
# SQL Server LocalDB
SqlLocalDB info
SqlLocalDB stop mssqllocaldb
# Copy database files
SqlLocalDB start mssqllocaldb
```

### Reset Database

```bash
# Drop and recreate
dotnet ef database drop --project BlogApp.Infrastructure --startup-project BlogApp
dotnet ef database update --project BlogApp.Infrastructure --startup-project BlogApp
```

### Database Integrity Checks

```sql
-- Check for orphaned foreign keys
DBCC CHECKDB (BlogAppDb);

-- Check for fragmentation
SELECT * FROM sys.dm_db_index_physical_stats(
	DB_ID('BlogAppDb'), NULL, NULL, NULL, 'LIMITED'
);
```

---

## Performance Considerations

### Indexes

Important indexes are configured in `OnModelCreating`:
- User-based queries (UserId)
- Time-based queries (CreatedOn)
- Foreign key columns

### Query Optimization

**Include Related Data:**
```csharp
var post = await _context.Posts
	.Include(p => p.Comments)
	.Include(p => p.Category)
	.FirstOrDefaultAsync();
```

**Pagination:**
```csharp
var posts = await _context.Posts
	.Skip((page - 1) * pageSize)
	.Take(pageSize)
	.ToListAsync();
```

### Connection Pooling

EF Core automatically uses connection pooling. Configured in DbContextOptions.

---

## See Also

- [SETUP.md](./SETUP.md) - Database configuration
- [ARCHITECTURE.md](../ARCHITECTURE.md) - Data layer architecture
- [DEVELOPMENT.md](./DEVELOPMENT.md) - Adding database features
