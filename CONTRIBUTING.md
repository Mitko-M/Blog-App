# Contributing Guidelines

Thank you for considering contributing to BlogApp! This document provides guidelines and instructions for contributing.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Process](#development-process)
- [Code Standards](#code-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Issue Reporting](#issue-reporting)

---

## Code of Conduct

### Our Commitment

We are committed to providing a welcoming and inclusive environment for all contributors.

### Expected Behavior

- Be respectful and inclusive
- Welcome different viewpoints and experiences
- Focus on constructive feedback
- Report unacceptable behavior to maintainers

### Unacceptable Behavior

- Harassment, discrimination, or abusive language
- Trolling or deliberate disruption
- Unwelcome sexual attention
- Publishing private information without consent

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- Visual Studio 2022 or VS Code
- Git
- SQL Server (LocalDB or Express)

### Setup Development Environment

```bash
# 1. Fork the repository
# Visit https://github.com/Mitko-M/Blog-App and click "Fork"

# 2. Clone your fork
git clone https://github.com/YOUR-USERNAME/Blog-App.git
cd blog-app

# 3. Add upstream remote
git remote add upstream https://github.com/Mitko-M/Blog-App.git

# 4. Create feature branch
git checkout -b feature/your-feature-name

# 5. Follow setup in SETUP.md
```

### Building the Project

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Run application
dotnet run --project BlogApp
```

---

## Development Process

### Feature Development Workflow

1. **Choose an Issue**
   - Look for issues tagged `good-first-issue` or `help-wanted`
   - Comment to express interest before starting
   - Create an issue for your idea first

2. **Create Feature Branch**
   ```bash
   git checkout -b feature/descriptive-feature-name
   ```

3. **Develop Locally**
   - Follow coding standards (see below)
   - Write tests for your changes
   - Keep commits atomic and logical

4. **Test Your Changes**
   ```bash
   # Run all tests
   dotnet test

   # Run specific test file
   dotnet test BlogApp.Core.Test/PostServiceTests.cs

   # Check code quality
   dotnet build /p:EnforceCodeStyleInBuild=true
   ```

5. **Update Documentation**
   - Add XML documentation to public methods
   - Update relevant `.md` files
   - Update CHANGELOG if applicable

6. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **Create Pull Request**
   - Compare your branch with upstream `development`
   - Fill out PR template completely
   - Wait for code review

---

## Code Standards

### Naming Conventions

Follow Microsoft C# Naming Conventions:

```csharp
// Classes and public methods: PascalCase
public class PostService { }
public async Task<Post> GetPostAsync(int id) { }

// Local variables and parameters: camelCase
var postTitle = "Hello";
public void UpdatePost(string postTitle) { }

// Constants: UPPER_CASE
public const int MaxPostLength = 5000;
```

### Code Style

**Formatting:**
- Indentation: 4 spaces (or 1 tab)
- Line length: ≤ 120 characters recommended
- Braces: Allman style (brace on new line)

**Example:**
```csharp
public class PostService
{
	public async Task AddPostAsync(AddPostFormModel model, string userId)
	{
		if (string.IsNullOrWhiteSpace(model.Title))
		{
			throw new ArgumentException("Title is required");
		}

		var post = new Post { Title = model.Title };
		await _context.Posts.AddAsync(post);
		await _context.SaveChangesAsync();
	}
}
```

### Documentation

Add XML documentation to all public methods:

```csharp
/// <summary>
/// Retrieves a post by its identifier.
/// </summary>
/// <param name="id">The post identifier.</param>
/// <returns>The post if found; otherwise, null.</returns>
/// <exception cref="ArgumentException">Thrown when id is invalid.</exception>
public async Task<Post?> GetPostById(int id)
{
	if (id <= 0)
		throw new ArgumentException("Id must be positive", nameof(id));

	return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
}
```

### Testing Requirements

- Add tests for new features
- Tests should follow AAA pattern (Arrange, Act, Assert)
- Test method names must be descriptive: `MethodName_Scenario_ExpectedResult`
- Aim for 80%+ code coverage on services

```csharp
[Test]
public async Task AddPostAsync_WithValidModel_SavesPostToDB()
{
	// Arrange
	var model = new AddPostFormModel { Title = "Test" };

	// Act
	await _postService.AddPostAsync(model, "userId");

	// Assert
	_mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
}
```

### Error Handling

- Handle exceptions appropriately
- Provide meaningful error messages
- Throw specific exception types

```csharp
try
{
	await _context.SaveChangesAsync();
}
catch (DbUpdateException ex)
{
	throw new InvalidOperationException("Failed to save post", ex);
}
```

### Performance Considerations

- Use async/await for all I/O operations
- Avoid N+1 queries with `.Include()`
- Use pagination for large datasets
- Add indexes to frequently queried columns

---

## Commit Guidelines

### Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

### Types

- **feat:** New feature
- **fix:** Bug fix
- **docs:** Documentation changes
- **style:** Formatting (no functional changes)
- **refactor:** Code restructuring (no functional changes)
- **test:** Adding or updating tests
- **chore:** Dependency updates, configuration changes

### Examples

**Good commits:**
```
feat: Add featured posts section to homepage

- Implement IPostService.GetFeaturedPostsAsync()
- Create FeaturedPostsComponent
- Add migration for Post.IsFeatured column
- Add unit tests

Fixes #123
```

```
fix: Prevent duplicate comments on post

- Add unique constraint check before inserting
- Return validation error if duplicate
- Add test case

Fixes #456
```

**Bad commits:**
```
Update code        # Too vague
Fixed stuff        # No detail
wip                # Work in progress should not be committed
```

### Commit Best Practices

1. **Atomic Commits:** One logical change per commit
2. **Frequent Commits:** Commit often (not at the end)
3. **Descriptive:** Provide context in commit message
4. **No Merge Commits:** Rebase before pushing
5. **No Large Commits:** Keep commits focused

```bash
# Good workflow
git add feature/part1
git commit -m "feat: Add post validation"

git add feature/part2
git commit -m "feat: Add migration for new column"

git add test/
git commit -m "test: Add tests for post validation"
```

---

## Pull Request Process

### Before Submitting

1. **Update your branch with latest upstream:**
   ```bash
   git fetch upstream
   git rebase upstream/development
   git push -f origin feature/your-feature
   ```

2. **Run all checks locally:**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   dotnet test
   ```

3. **Update documentation**
   - CHANGELOG.md
   - Relevant .md files in /docs
   - XML documentation in code

### Pull Request Template

```markdown
## Description
Brief description of changes

## Related Issue
Fixes #123

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests added
- [ ] All tests passing
- [ ] Code coverage maintained

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-reviewed my own code
- [ ] Commented complex logic
- [ ] Updated documentation
- [ ] No new warnings generated
- [ ] Tests pass locally
```

### Review Process

1. **Reviewer Assignment**
   - Project maintainers will review your PR
   - Typically within 1-2 weeks

2. **Code Review Focus**
   - Correctness of implementation
   - Code quality and standards
   - Test coverage
   - Documentation completeness
   - Performance implications

3. **Addressing Feedback**
   - Make requested changes
   - Commit with clear message
   - Re-request review
   - Be respectful and constructive

4. **Approval & Merge**
   - After approval, maintainers will merge
   - Your feature branch can be deleted

---

## Issue Reporting

### Before Opening an Issue

- Search existing issues (open and closed)
- Check documentation (README, docs/)
- Try reproducing with latest code
- Check troubleshooting guide

### Issue Template

```markdown
## Bug Report / Feature Request
[Select one]

## Description
Clear description of the issue

## Steps to Reproduce (Bug)
1. Step 1
2. Step 2
3. Step 3

## Expected Behavior
What should happen

## Actual Behavior
What actually happens

## Environment
- OS: [Windows 10, Ubuntu, macOS]
- .NET Version: 10.0
- Browser: [Chrome 120, Edge, etc.]

## Screenshots (if applicable)
Attach screenshots

## Additional Context
Any other relevant information

## Logs
```
Paste any error logs or stack traces
```
```

### Labels

Issues will be labeled by maintainers:
- `bug` - Bug report
- `enhancement` - Feature request
- `documentation` - Docs improvement
- `good-first-issue` - Good for new contributors
- `help-wanted` - Extra attention needed
- `wontfix` - Deliberately not being fixed

---

## Development Tips

### Useful Commands

```bash
# Format code (install dotnet-format first)
dotnet tool install -g dotnet-format
dotnet format

# Run code analyzers
dotnet build /p:EnforceCodeStyleInBuild=true

# Run single test
dotnet test --filter TestMethod=TestMethodName

# Debug tests in VS Code
dotnet test --debug

# Generate coverage report
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

### Debugging in Visual Studio

1. Set breakpoint in code
2. Right-click file in Solution Explorer
3. Select "Debug Tests"
4. Use Debug toolbar to step through

### Common Issues

**Issue:** "NuGet packages restored but build fails"
```bash
dotnet clean
dotnet restore
dotnet build
```

**Issue:** "Tests pass locally but fail in CI"
- Check for hardcoded paths
- Verify test data is seeded
- Check database connection string

**Issue:** "Migration conflicts with other PRs"
- Rebase on latest development branch
- Rename migration if needed
- Coordinate with team

---

## Recognition

### Contributors

All contributors will be recognized in:
- CHANGELOG.md (for features/fixes)
- GitHub Contributors page
- Project README (maintainers)

### Levels

- **Committer:** Merged PRs with code changes
- **Maintainer:** Active contributors with merge rights
- **Core Team:** Long-term maintainers

---

## Resources

- [DEVELOPMENT.md](./DEVELOPMENT.md) - Development guide
- [TESTING.md](./TESTING.md) - Testing guide
- [ARCHITECTURE.md](../ARCHITECTURE.md) - Architecture
- [GitHub Issues](https://github.com/Mitko-M/Blog-App/issues)
- [GitHub Discussions](https://github.com/Mitko-M/Blog-App/discussions)

---

## Questions?

- Check existing discussions
- Open a new discussion
- Comment on relevant issue
- Contact maintainers

---

**Thank you for contributing to BlogApp! 🎉**
