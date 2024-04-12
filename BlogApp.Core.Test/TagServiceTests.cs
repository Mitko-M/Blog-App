using BlogApp.Core.Contracts;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class TagServiceTests
    {
        private BlogAppDbContext context;
        private ITagService tagService;

        [SetUp]
        public void SetUp()
        {
            var tags = new List<Tag>()
            {
                new Tag()
                {
                    Id = 1,
                    Name = "TestTag1",
                },
                new Tag()
                {
                    Id = 2,
                    Name = "TestTag2",
                },
                new Tag()
                {
                    Id = 3,
                    Name = "TestTag3",
                }
            };

            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "TagInMemoryDatabase")
                .Options;

            context = new BlogAppDbContext(options);

            context.Tags.AddRange(tags);
            context.SaveChanges();

            tagService = new TagService(context);
        }

        [Test]
        public async Task TestingGetTagsAsyncMethod()
        {
            int count = 3;

            var tags = await tagService.GetTagsAsync();
            int tagCount = tags.Count();

            Assert.AreEqual(count, tagCount);
        }

        [Test]
        public async Task TestingGetTagsWithIsSelectedMethod()
        {
            int count = 3;

            var tags = await tagService.GetTagsWithIsSelected();
            int tagCount = tags.Count();

            Assert.AreEqual(count, tagCount);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
            tagService = null;
        }
    }
}
