using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Models.Report;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class PostServiceTests
    {
        private IEnumerable<Post> posts;
        private IEnumerable<PostCategoryFormModel> categories;
        private IEnumerable<PostTagFormModel> tags;
        private Mock<ICategoryService> mockCategoryService;
        private Mock<ITagService> mockTagService;
        private IPostService postService;
        private BlogAppDbContext context;

        [SetUp]
        public void Setup()
        {
            categories = new List<PostCategoryFormModel>()
            {
                new PostCategoryFormModel()
                {
                    Id = 1,
                    Name = "TestCat1",
                    IsSelected = false,
                },
                new PostCategoryFormModel()
                {
                    Id = 2,
                    Name = "TestCat2",
                    IsSelected = false,
                },
                new PostCategoryFormModel()
                {
                    Id = 3,
                    Name = "TestCat3",
                    IsSelected = false,
                }
            };
            tags = new List<PostTagFormModel>()
            {
                new PostTagFormModel()
                {
                    Id = 1,
                    Name = "TestTag1",
                    IsSelected = false
                },
                new PostTagFormModel()
                {
                    Id = 2,
                    Name = "TestTag2",
                    IsSelected = false
                },
                new PostTagFormModel()
                {
                    Id = 3,
                    Name = "TestTag3",
                    IsSelected = false
                }
            };

            mockCategoryService = new Mock<ICategoryService>();
            mockTagService = new Mock<ITagService>();

            mockCategoryService
                .Setup(service => service.GetCategoriesWithIsSelected())
                .Returns(Task.FromResult(categories));

            mockTagService
                .Setup(service => service.GetTagsWithIsSelected())
                .Returns(Task.FromResult(tags));

            posts = new List<Post>()
            {
                new Post()
                {
                    Id = 1,
                    Title = "My First Post",
                    Content = "This is my first post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddMonths(-60),
                    UpdatedOn = DateTime.Now.AddMonths(-30),
                    UserId = "the_admin_id",
                },
                new Post()
                {
                    Id = 2,
                    Title = "My Second Post",
                    Content = "This is my second post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddYears(-5),
                    UpdatedOn = DateTime.Now.AddMonths(-10),
                    UserId = "the_admin_id",
                },
                new Post()
                {
                    Id = 3,
                    Title = "My Third Post",
                    Content = "This is my third post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddDays(-60),
                    UpdatedOn = DateTime.Now.AddDays(-5),
                    UserId = "the_admin_id",
                }
            };

            var adminUser = new ApplicationUser()
            {
                Id = "the_admin_id",
                FirstName = "Mitko",
                LastName = "Mitkov",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@blog.com",
                NormalizedEmail = "ADMIN@BLOG.COM"
            };


            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "PostInMemoryDatabase")
                .Options;

            context = new BlogAppDbContext(options);
            context.Posts.AddRange(posts);
            context.Users.Add(adminUser);
            context.SaveChanges();

            postService = new PostService(context, mockCategoryService.Object, mockTagService.Object);
        }

        [Test]
        public void TestingTheAddPostAsyncMethod()
        {
            //arrange
            int postsCount = 4;

            //act
            string userId = "the_admin_id";

            var post = new AddPostFormModel()
            {
                Title = "My First Post",
                Content = "This is my first post's content",
                ShortDescription = "This is my post's short description",
                Categories = new List<PostCategoryFormModel>(),
                Tags = new List<PostTagFormModel>()
            };

            postService.AddPostAsync(post, userId);

            int actualPostCount = context.Posts.Count();

            //assert
            Assert.AreEqual(postsCount, actualPostCount,
                message: "A post wasn't successfully added with AddPostAsync method");

            Assert.DoesNotThrow(() => context.SaveChangesAsync(),
                message: "Method AddPostAsync threw exception on SaveChangesAsync");
        }

        [Test]
        public async Task TestingTheGetPostFormModelMethod()
        {
            //arrange
            var model = new AddPostFormModel()
            {
                Categories = categories,
                Tags = tags
            };

            //act
            var actualModel = await postService.GetPostFormModel();

            bool carCheck = false;
            bool tagCheck = false;

            foreach (var cat in actualModel.Categories)
            {
                if (model.Categories.Contains(cat))
                {
                    carCheck = true;
                }
                else
                {
                    carCheck = false;
                }
            }

            foreach (var tag in actualModel.Tags)
            {
                if (model.Tags.Contains(tag))
                {
                    tagCheck = true;
                }
                else
                {
                    tagCheck = false;
                }
            }

            //assert
            Assert.True(tagCheck && carCheck, message: "The method GetPostFormModel returns wrong model");
        }

        //TODO: Test the methods listed below in integration tests with real database
        //Methods that can't be unit tested:
        // GetAllPostsAsync, GetMinePostsAsync, GetPostByIdAsync,
        //GetPostToEditAsync

        //I can't test these methods since they use the
        //Include and ThenInclude methods and they are relational database methods
        //while the Entity Framework Core InMemory database is non-relational

        [Test]
        public async Task TestingGetPostToEditAsyncMethod()
        {
            //arrange
            var post = posts.First();
            int id = post.Id;

            //act
            var returnedPostModel = await postService.GetPostToEditAsync(id, post);

            //assert
            Assert.AreEqual(post.Title, returnedPostModel.Title);
        }

        [Test]
        public async Task TestingUpdatePostAsyncMethod()
        {
            //arrange
            var post = posts.First();
            int id = post.Id;
            var model = new AddPostFormModel();

            //act
            await postService.UpdatePostAsync(post, model);

            //assert
            string title = context.Posts.FirstOrDefault(p => p.Id == id).Title;

            Assert.AreEqual(string.Empty, title,
                message: "Post with id {0} wasn't updated successfully with empty model", id);
        }

        [Test]
        public async Task TestingDeletePostAsyncMethod()
        {
            //arrange
            var postToDelete = posts.First();
            int postCount = 2;

            //act
            await postService.DeletePostAsync(postToDelete);

            //assert
            int actualPostCount = context.Posts.Count();

            Assert.AreEqual(postCount, actualPostCount);
        }

        [Test]
        public void TestingGetPostDetailsViewModelMethod()
        {
            //arrange
            var post = posts.First();
            
            //act
            var postViewModel = postService.GetPostDetailsViewModel(post);

            //assert
            Assert.AreEqual(post.Id, postViewModel.Id);
        }

        [Test]
        public async Task TestingReportPostMethod()
        {
            //arrange
            int reportCount = 1;
            var post = posts.First();
            var model = new PostReportViewModel()
            {
                PostId = 1,
                UserId = "the_admin_id",
                ReportContent = "Unit testing report"
            };

            //act
            await postService.ReportPost(model, post);

            //assert
            int actualReportCount = context.PostsReports.Count();
            Assert.AreEqual(reportCount, actualReportCount);
        }

        [TearDown]
        public void TearDown()
        {
            posts = null;
            mockCategoryService = null;
            categories = null;
            tags = null;
            mockTagService = null;
            postService = null;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}