using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private BlogAppDbContext context;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                FirstName = "Mitko",
                LastName = "Mitkov",
                Email = "email@gmail.com",
                UserName = "testUser",
                PhoneNumber = "1234567890",
                Banned = true
            };

            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
               .UseInMemoryDatabase(databaseName: "UserInMemoryDatabase")
               .Options;

            context = new BlogAppDbContext(options);

            context.Users.Add(user);

            userService = new UserService(context);
        }

        [Test]
        public async Task TestingGetUserByIdMethod()
        {
            var user = await userService.GetUserById("1");

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public async Task TestingIsUserBannedMethod()
        {
            bool banned = true;

            var bannedUser = await userService.IsUserBanned("1");

            Assert.That(banned, Is.EqualTo(bannedUser));
        }

        [Test]
        public async Task TestingUpdateUserDataMethod()
        {
            var model = new ApplicationUserViewModel()
            {
                FirstName = "Mitko1",
                LastName = "Mitkov1",
                Email = "email@gmail.com123",
                UserName = "testUser123",
                PhoneNumber = "1234567890123"
            };

            await userService.UpdateUserData(model, "1");

            var user = context.Users.Find("1");

            Assert.That(model.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(model.LastName, Is.EqualTo(user.LastName));
            Assert.That(model.Email, Is.EqualTo(user.Email));
            Assert.That(model.UserName, Is.EqualTo(user.UserName));
        }

        [TearDown]
        public void TearDown()
        {
            userService = null;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
