using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private BlogAppDbContext context;
        private ICategoryService categoryService;

        [SetUp]
        public void SetUp()
        {
            var cats = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "TestCat1",
                },
                new Category()
                {
                    Id = 2,
                    Name = "TestCat2",
                },
                new Category()
                {
                    Id = 3,
                    Name = "TestCat3",
                }
            };

            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryInMemoryDatabase")
                .Options;

            context = new BlogAppDbContext(options);

            context.Categories.AddRange(cats);
            context.SaveChanges();

            categoryService = new CategoryService(context);
        }

        [Test]
        public async Task TestingGetCategoriesAsyncMethod()
        {
            //arrange
            int count = 3;

            //act
            var cats = await categoryService.GetCategoriesAsync();

            int catCount = cats.Count();

            //assert
            Assert.AreEqual(count, catCount, message: "Wrond category numbers");
        }

        [Test]
        public async Task TestingGetCategoriesWithIsSelectedMethod()
        {
            //arrange
            int count = 3;

            //act
            var cats = await categoryService.GetCategoriesWithIsSelected();
            int catCount = cats.Count();

            //assert
            Assert.AreEqual(count, catCount);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
            categoryService = null;
        }
    }
}
