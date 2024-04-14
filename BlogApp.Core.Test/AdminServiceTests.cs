using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class AdminServiceTests
    {
        private BlogAppDbContext context;
        private IAdminService adminService;
        private Mock<IUserService> mockUserService;

        [SetUp]
        public void SetUp()
        {
            var users = new ApplicationUser[]
            {
                new ApplicationUser()
                {
                    Id = "adminId",
                    UserName = "admin",
                    Banned = false
                },
                new ApplicationUser()
                {
                    Id = "userId",
                    UserName = "user",
                    Banned = true
                },
            };
            var post = new Post()
            {
                Id = 1,
                UserId = "adminId"
            };
            var roles = new IdentityRole[]
            {
                new IdentityRole()
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            var userRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>()
                {
                    UserId = "adminId",
                    RoleId = "1"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "userId",
                    RoleId = "2"
                }
            };
            var contactForm = new ContactFormEntry()
            {
                Id = 1,
                UserId = "admin",
                Subject = "TestSubject",
                Message = "TestMessage",
                CreatedOn = DateTime.Now,
                Name = "Mitko Mitkov",
                Email = "email@email.com",
                User = users[0]
            };
            var report = new PostReport()
            {
                Id = 1,
                ReportContent = "reporting",
                UserId = "userId",
                PostId = 1,
                User = users[1]
            };

            var userWithDetails = new ApplicationUserWithAllDataViewModel()
            {
                FirstName = users[0].FirstName,
                LastName = users[0].LastName,
                UserName = users[0].UserName,
                Email = users[0].Email,
                Role = "Admin",
                PostCount = 1,
                PhoneNumber = "123456789",
                Banned = false,
                WarningsCount = 0,
                Posts = new List<PostDetailsViewModel>()
            };

            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminServiceInMemoryDb")
                .Options;

            context = new BlogAppDbContext(options);

            context.Users.AddRange(users);
            context.Roles.AddRange(roles);
            context.UserRoles.AddRange(userRoles);
            context.ContactFormEntries.Add(contactForm);
            context.Posts.Add(post);
            context.PostsReports.Add(report);

            context.SaveChanges();

            mockUserService = new Mock<IUserService>();

            mockUserService
                .Setup(service => service.GetUserById("adminId"))
                .Returns(Task.FromResult(userWithDetails));

            adminService = new AdminService(context, mockUserService.Object);
        }

        [Test]
        public async Task TestingBannMethod()
        {
            string userName = "admin";

            await adminService.Bann(userName);
            var user = context.Users.First();

            Assert.IsTrue(user.Banned);
        }

        [Test]
        public async Task TestingDeleteContactFormEntryMethod()
        {
            int id = 1;
            int count = 0;

            await adminService.DeleteContactFormEntry(id);
            int actualCount = context.ContactFormEntries.Count();

            Assert.AreEqual(count, actualCount);
        }

        [Test]
        public async Task TestingDeleteReportMethod()
        {
            int count = 0;
            int id = 1;

            await adminService.DeleteReport(id);
            int actualCount = context.PostsReports.Count();

            Assert.AreEqual(count, actualCount);
        }

        [Test]
        public async Task TestingGetAdminsAsyncMethod()
        {
            int count = 1;

            var admins = await adminService.GetAdminsAsync();

            Assert.AreEqual(count, admins.Count());
        }

        [Test]
        public async Task TestingGetAllContactFormsAsyncMethod()
        {
            int count = 1;

            var contactForms = await adminService.GetAllContactFormsAsync();

            Assert.AreEqual(count, contactForms.Count());
        }

        [Test]
        public async Task TestingGetAllReportsAsyncMethod()
        {
            int count = 1;

            var reports = await adminService.GetAllReportsAsync();

            Assert.AreEqual(count, reports.Count());
        }

        [Test]
        public async Task TestingGetAllUsersAsyncMethod()
        {
            int count = 2;

            var users = await adminService.GetAllUsersAsync();

            Assert.AreEqual(count, users.Count());
        }

        [Test]
        public async Task TestingGetContactFormByIdMethod()
        {
            var contactForm = await adminService.GetContactFormById(1);

            Assert.IsNotNull(contactForm);
        }

        [Test]
        public async Task TestingGetReportByIdMethod()
        {
            var report = await adminService.GetReportById(1);

            Assert.IsNotNull(report);
        }

        [Test]
        public async Task TestingGetUsersAsyncMethod()
        {
            int count = 1;

            var users = await adminService.GetUsersAsync();

            Assert.AreEqual(count, users.Count());
        }

        [Test]
        public async Task TestingGetUsersOnRoleNameAsyncMethod()
        {
            int count = 1;
            string roleName = "Admin";

            var users = await adminService.GetUsersOnRoleNameAsync(roleName);

            Assert.AreEqual(count, users.Count());
        }

        [Test]
        public async Task TestingManageUserByUserNameMethod()
        {
            var user = await adminService.ManageUserByUserName("admin");

            Assert.IsNotNull(user);
        }

        [Test]
        public async Task TestingUnBannMethod()
        {
            string userName = "user";

            await adminService.UnBann(userName);
            var user = context.Users.First(u => u.Id == "userId");

            Assert.IsFalse(user.Banned);
        }

        [Test]
        public async Task TestingWarnApplicationUserMethod()
        {
            int count = 1;
            int reportId = 1;
            int postId = 1;
            string userId = "adminId";
            int reportCount = 0;

            await adminService.WarnApplicationUser(reportId, postId, userId);

            var user = context.Users.First(u => u.Id == "adminId");
            int warnings = user.Warnings.Count();
            int actualReportCount = context.PostsReports.Count();
            var post = context.Posts.First();

            Assert.AreEqual(count, warnings);
            Assert.AreEqual(reportCount, actualReportCount);
            Assert.IsTrue(post.Hidden);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
            adminService = null;
            mockUserService = null;
        }
    }
}
