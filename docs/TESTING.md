# Testing Guide

Comprehensive guide for testing in BlogApp.

## Table of Contents

- [Testing Overview](#testing-overview)
- [Test Framework Setup](#test-framework-setup)
- [Unit Testing](#unit-testing)
- [Writing Tests](#writing-tests)
- [Running Tests](#running-tests)
- [Mocking & Fixtures](#mocking--fixtures)
- [Best Practices](#best-practices)
- [Coverage](#coverage)

---

## Testing Overview

BlogApp uses **NUnit** as the testing framework and **Moq** for mocking dependencies.

**Test Project:** `BlogApp.Core.Test`

**Test Scope:**
- Unit tests for service layer (business logic)
- Database context mocking
- Service method validation
- Error handling verification

**NOT Tested (currently):**
- UI/View testing (integration tests)
- End-to-end scenarios
- Controller actions (can be added)

---

## Test Framework Setup

### Project Structure

```
BlogApp.Core.Test/
├── AdminServiceTests.cs
├── CategoryServiceTests.cs
├── CommentServiceTest.cs
├── ContactServiceTests.cs
├── PostServiceTests.cs
├── TagServiceTests.cs
├── UserServiceTests.cs
└── BlogApp.Core.Test.csproj
```

### NuGet Packages

```xml
<!-- BlogApp.Core.Test.csproj -->
<ItemGroup>
	<PackageReference Include="NUnit" Version="4.x" />
	<PackageReference Include="Moq" Version="4.x" />
	<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.x" />
</ItemGroup>
```

### Test Conventions

1. **Test Class Name:** `{ServiceName}Tests`
2. **Test Method Name:** `{MethodName}_{Scenario}_{ExpectedResult}`
3. **One Assertion Per Test:** Fail fast approach
4. **AAA Pattern:** Arrange, Act, Assert

---

## Unit Testing

### What to Test

**Service Methods:**
```csharp
[TestClass]
public class PostServiceTests
{
	// Test these scenarios:
	// 1. Happy path (successful execution)
	// 2. Invalid input
	// 3. Empty/null results
	// 4. Database errors
	// 5. Authorization checks
}
```

### Test Structure

```csharp
[TestFixture]
public class PostServiceTests
{
	// Setup variables
	private Mock<BlogAppDbContext> _mockContext;
	private IPostService _postService;

	[SetUp]
	public void Setup()
	{
		// Initialize mocks before each test
		_mockContext = new Mock<BlogAppDbContext>();
		_postService = new PostService(_mockContext.Object);
	}

	[Test]
	public async Task GetPostById_WithValidId_ReturnsPost()
	{
		// Arrange
		var postId = 1;
		var expectedPost = new Post { Id = postId, Title = "Test" };

		// Act
		var result = await _postService.GetPostById(postId);

		// Assert
		Assert.AreEqual(expectedPost.Id, result?.Id);
	}
}
```

---

## Writing Tests

### Basic Test Example

```csharp
[Test]
public async Task AddPostAsync_WithValidModel_SavesPost()
{
	// Arrange
	var model = new AddPostFormModel
	{
		Title = "Test Post",
		Content = "<p>Test content</p>",
		ShortDescription = "Short desc"
	};
	var userId = "user123";

	// Act
	await _postService.AddPostAsync(model, userId);

	// Assert
	_mockContext.Verify(
		c => c.Posts.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()),
		Times.Once
	);
}
```

### Testing Error Conditions

```csharp
[Test]
public void AddPostAsync_WithEmptyTitle_ThrowsArgumentException()
{
	// Arrange
	var model = new AddPostFormModel
	{
		Title = "", // Invalid
		Content = "Content",
		ShortDescription = "Desc"
	};

	// Act & Assert
	Assert.ThrowsAsync<ArgumentException>(
		() => _postService.AddPostAsync(model, "user123")
	);
}
```

### Testing Collections

```csharp
[Test]
public async Task GetAllPostsAsync_ReturnsCorrectNumberOfPosts()
{
	// Arrange
	var posts = new List<Post>
	{
		new Post { Id = 1, Title = "Post 1" },
		new Post { Id = 2, Title = "Post 2" }
	}.AsQueryable();

	// Act
	var result = await _postService.GetAllPostsAsync();

	// Assert
	Assert.AreEqual(2, result.Posts.Count());
	Assert.IsTrue(result.Posts.Any(p => p.Title == "Post 1"));
}
```

### Testing Async Operations

```csharp
[Test]
public async Task UpdatePostAsync_WithValidData_UpdatesPost()
{
	// Arrange
	var post = new Post { Id = 1, Title = "Old Title" };
	var model = new AddPostFormModel { Title = "New Title" };

	// Act
	await _postService.UpdatePostAsync(post, model);

	// Assert
	Assert.AreEqual("New Title", post.Title);
	_mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
}
```

---

## Running Tests

### Run All Tests

```bash
# From solution root
dotnet test

# With verbose output
dotnet test --verbosity detailed

# Show test names as they run
dotnet test --logger "console;verbosity=detailed"
```

### Run Specific Test Class

```bash
dotnet test --filter TestClass=BlogApp.Core.Test.PostServiceTests
```

### Run Specific Test Method

```bash
dotnet test --filter TestMethod=GetAllPostsAsync_ReturnsCorrectPosts
```

### Run with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

### Debugging Tests

In Visual Studio:
1. Set breakpoint in test
2. Right-click test → Debug Test
3. Use Debug toolbar to step through

Or in VS Code:
```bash
dotnet test --debug
# Then attach debugger
```

---

## Mocking & Fixtures

### Mocking DbContext

```csharp
// Setup mock DbContext
var mockContext = new Mock<BlogAppDbContext>();

// Mock DbSet
var posts = new List<Post>
{
	new Post { Id = 1, Title = "Test" }
}.AsQueryable();

mockContext
	.Setup(c => c.Posts)
	.Returns(MockDbSet(posts).Object);

// Create service with mock
var service = new PostService(mockContext.Object);
```

### Helper: Mock DbSet

```csharp
public static Mock<DbSet<T>> MockDbSet<T>(IQueryable<T> data) where T : class
{
	var mockSet = new Mock<DbSet<T>>();

	mockSet.As<IAsyncEnumerable<T>>()
		.Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
		.Returns(new AsyncEnumerator<T>(data.GetEnumerator()));

	mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
	mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
	mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
	mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

	return mockSet;
}
```

### Test Fixtures

```csharp
[TestFixture]
public class PostServiceTests
{
	// Shared test data
	private List<Post> GetSamplePosts()
	{
		return new List<Post>
		{
			new Post { Id = 1, Title = "Post 1", CreatedOn = DateTime.Now },
			new Post { Id = 2, Title = "Post 2", CreatedOn = DateTime.Now.AddDays(-1) }
		};
	}

	private AddPostFormModel GetSampleFormModel()
	{
		return new AddPostFormModel
		{
			Title = "Sample Post",
			Content = "<p>Sample</p>",
			ShortDescription = "Sample desc"
		};
	}
}
```

### Moq Verification

```csharp
// Verify method was called
_mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

// Verify with specific arguments
_mockContext.Verify(
	c => c.Posts.AddAsync(
		It.Is<Post>(p => p.Title == "Test"),
		It.IsAny<CancellationToken>()
	),
	Times.Once
);

// Verify method was never called
_mockContext.Verify(c => c.SaveChangesAsync(), Times.Never);

// Verify call count
_mockContext.Verify(c => c.SaveChangesAsync(), Times.Exactly(2));
```

---

## Best Practices

### Naming Clarity

```csharp
// ✓ Good - Clear what is tested
public void GetAllPostsAsync_WithCategoryFilter_ReturnsPostsInCategory()

// ✗ Bad - Unclear
public void Test1()
```

### One Assertion Per Test

```csharp
// ✓ Good - Single assertion
[Test]
public void GetPostById_ReturnsPost()
{
	var result = _postService.GetPostById(1);
	Assert.IsNotNull(result);
}

[Test]
public void GetPostById_ReturnsCorrectTitle()
{
	var result = _postService.GetPostById(1);
	Assert.AreEqual("Expected Title", result.Title);
}

// ✗ Bad - Multiple assertions
[Test]
public void GetPostById_Works()
{
	var result = _postService.GetPostById(1);
	Assert.IsNotNull(result);
	Assert.AreEqual("Title", result.Title);
	Assert.IsTrue(result.Id > 0);
}
```

### Independent Tests

```csharp
// ✓ Good - Tests are independent
[Test]
public void Test1() { /* ... */ }

[Test]
public void Test2() { /* ... */ } // Doesn't depend on Test1

// ✗ Bad - Test depends on order
[Test]
public void Test1_CreatesData() { /* ... */ }

[Test]
public void Test2_UsesDataFromTest1() { /* ... */ } // Will fail if run alone
```

### Use Descriptive Assertions

```csharp
// ✓ Good
Assert.AreEqual(expectedCount, actualCount, "Post count should match");

// Moq verification
_mockContext.Verify(
	c => c.Posts.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()),
	Times.Once,
	"Post should be added to database"
);

// ✗ Bad
Assert.AreEqual(5, result.Count());
```

### Mock External Dependencies Only

```csharp
// ✓ Good - Only mock database
public class PostServiceTests
{
	private Mock<BlogAppDbContext> _mockContext; // Mock
	private PostService _postService; // Real

	[SetUp]
	public void Setup()
	{
		_mockContext = new Mock<BlogAppDbContext>();
		_postService = new PostService(_mockContext.Object); // Test real logic
	}
}

// ✗ Bad - Don't mock the class under test
public class PostServiceTests
{
	private Mock<PostService> _mockService; // Wrong!
}
```

### Test Boundary Conditions

```csharp
[TestFixture]
public class CommentServiceTests
{
	[Test]
	public void AddCommentAsync_WithMinimumLength_Succeeds()
	{
		// 1 character minimum
		var comment = new CommentFormModel { Content = "A" };
		Assert.DoesNotThrowAsync(() => _service.AddCommentAsync(comment));
	}

	[Test]
	public void AddCommentAsync_WithMaximumLength_Succeeds()
	{
		// 100 character maximum
		var comment = new CommentFormModel { Content = new string('x', 100) };
		Assert.DoesNotThrowAsync(() => _service.AddCommentAsync(comment));
	}

	[Test]
	public void AddCommentAsync_ExceedsMaxLength_Throws()
	{
		var comment = new CommentFormModel { Content = new string('x', 101) };
		Assert.ThrowsAsync<ArgumentException>(() => _service.AddCommentAsync(comment));
	}
}
```

---

## Coverage

### Current Coverage

Run tests with coverage reporting:

```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover /p:Exclude="[*Tests]*"
```

**Services Covered:**
- PostService - Most methods
- CommentService - Most methods
- AdminService - Most methods
- CategoryService - Most methods
- TagService - Most methods
- ContactService - Most methods
- UserService - Most methods

### Improving Coverage

1. **Identify Untested Code:**
   ```bash
   # Review coverage report in ./coverage directory
   ```

2. **Write Tests for Gap:** 
   - Error paths
   - Edge cases
   - Null handling

3. **Example: Missing Test**
   ```csharp
   // Add test for exception handling
   [Test]
   public void DeletePostAsync_WhenDbThrows_ThrowsApplicationException()
   {
	   // Arrange
	   var post = new Post { Id = 1 };
	   _mockContext
		   .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
		   .ThrowsAsync(new DbUpdateException("DB error"));

	   // Act & Assert
	   Assert.ThrowsAsync<ApplicationException>(
		   () => _postService.DeletePostAsync(post)
	   );
   }
   ```

### Coverage Goals

- **Service Layer:** 80%+ coverage target
- **Models:** 100% (data models should be simple)
- **Controllers:** Covered via integration tests (future)
- **Utilities:** 100% if simple, 80%+ if complex

---

## Test Examples by Service

### PostService Tests

```csharp
[TestFixture]
public class PostServiceTests
{
	private Mock<BlogAppDbContext> _mockContext;
	private Mock<ICategoryService> _mockCategoryService;
	private Mock<ITagService> _mockTagService;
	private PostService _postService;

	[SetUp]
	public void Setup()
	{
		_mockContext = new Mock<BlogAppDbContext>();
		_mockCategoryService = new Mock<ICategoryService>();
		_mockTagService = new Mock<ITagService>();

		_postService = new PostService(
			_mockContext.Object,
			_mockCategoryService.Object,
			_mockTagService.Object
		);
	}

	[Test]
	public async Task GetAllPostsAsync_NoFilter_ReturnsAllPosts()
	{
		// Arrange
		var posts = new List<Post>
		{
			new Post { Id = 1, Title = "Post 1" },
			new Post { Id = 2, Title = "Post 2" }
		}.AsQueryable();

		// Mock...

		// Act
		var result = await _postService.GetAllPostsAsync();

		// Assert
		Assert.AreEqual(2, result.Posts.Count());
	}
}
```

### CommentService Tests

```csharp
[TestFixture]
public class CommentServiceTests
{
	private Mock<BlogAppDbContext> _mockContext;
	private CommentService _commentService;

	[SetUp]
	public void Setup()
	{
		_mockContext = new Mock<BlogAppDbContext>();
		_commentService = new CommentService(_mockContext.Object);
	}

	[Test]
	public async Task AddCommentAsync_WithValidModel_SavesComment()
	{
		// Arrange
		var model = new CommentFormModel
		{
			Content = "Great post!",
			PostId = 1,
			UserId = "user1"
		};

		// Act
		await _commentService.AddCommentAsync(model);

		// Assert
		_mockContext.Verify(
			c => c.Comments.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()),
			Times.Once
		);
	}
}
```

---

## Continuous Integration

### GitHub Actions Setup

```yaml
# .github/workflows/tests.yml
name: Tests

on: [push, pull_request]

jobs:
  test:
	runs-on: ubuntu-latest
	steps:
	- uses: actions/checkout@v2
	- uses: actions/setup-dotnet@v1
	  with:
		dotnet-version: '10.0.x'
	- run: dotnet restore
	- run: dotnet build
	- run: dotnet test
```

---

## See Also

- [DEVELOPMENT.md](./DEVELOPMENT.md) - Adding testable code
- [API.md](./API.md) - Service methods to test
- [CONTRIBUTING.md](./CONTRIBUTING.md) - Pull request testing requirements
