using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class CommentServiceTest
    {
        private BlogAppDbContext context;
        private ICommentService commentService;
        private Mock<IPostService> mockPostService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentServiceInMemoryDb")
                .Options;

            context = new BlogAppDbContext(options);

            var user = new ApplicationUser()
            {
                Id = "adminId",
                UserName = "admin"
            };

            var post = new Post()
            {
                Id = 1,
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        Id = 1,
                        Content = "TestComment1",
                        UserId = "adminId",
                        User = user,
                        PostId = 1,
                        CommentUploadDate = DateTime.Now,
                    },
                    new Comment()
                    {
                        Id = 2,
                        Content = "TestComment2",
                        UserId = "adminId",
                        PostId = 2,
                        CommentUploadDate = DateTime.Now,
                    },
                    new Comment()
                    {
                        Id = 3,
                        Content = "TestComment3",
                        UserId = "adminId",
                        PostId = 3,
                        CommentUploadDate = DateTime.Now,
                    },
                },
                User = user
            };

            var commentLike = new CommentLike()
            {
                Id= 1,
                CommentId = 1,
                UserId = "adminId"
            };

            context.Posts.Add(post);
            context.CommentsLikes.Add(commentLike);
            context.SaveChanges();

            mockPostService = new Mock<IPostService>();

            mockPostService
            .Setup(service => service.GetPostById(1))
            .Returns(Task.FromResult(post));


            commentService = new CommentService(context, mockPostService.Object);
        }

        [Test]
        public async Task TestingAddCommentAsyncMethod()
        {
            int count = 4;
            var model = new CommentFormModel()
            {
                Content = "TestContent",
                UserId = "adminId",
                PostId = 1,
                CommentUploadDate = DateTime.Now,
                Liked = false
            };

            await commentService.AddCommentAsync(model);
            int actualCount = context.Comments.Count();

            Assert.AreEqual(count, actualCount);
        }

        [Test]
        public async Task TestingLoadCommentsAsyncMethod()
        {
            int count = 3;

            int actualCount = (await commentService.LoadCommentsAsync(1)).Count();

            Assert.AreEqual(count, actualCount);
        }

        [Test]
        public async Task TestingLikeCommentMethod()
        {
            int count = 2;

            await commentService.LikeComment(1, "adminId");
            int actualCount = context.CommentsLikes.Count();

            Assert.AreEqual(count, actualCount);
        }

        [Test]
        public async Task TestingUnlikeCommentMethod()
        {
            int count = 0;

            await commentService.UnlikeComment(1, "adminId");
            int actualCount = context.CommentsLikes.Count();

            Assert.AreEqual(count, actualCount);
        }

        [TearDown]
        public void TearDown()
        {
            commentService = null;
            mockPostService = null;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
