using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Contact;
using BlogApp.Core.Services;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Test
{
    [TestFixture]
    public class ContactServiceTests
    {
        private BlogAppDbContext context;
        private IContactService contactService;

        [SetUp]
        public void SetUp()
        {
            var contactForms = new List<ContactFormEntry>()
            {
                new ContactFormEntry()
                {
                    Id = 1,
                    UserId = "adminId",
                    Name = "TestContactForm1",
                    Email = "TestEmail1",
                    Subject = "TestSubject1",
                    Message = "TestMessage1",
                    CreatedOn = DateTime.Now,
                },
                new ContactFormEntry()
                {
                    Id = 2,
                    UserId = "adminId",
                    Name = "TestContactForm2",
                    Email = "TestEmail2",
                    Subject = "TestSubject2",
                    Message = "TestMessage2",
                    CreatedOn = DateTime.Now,
                },
                new ContactFormEntry()
                {
                    Id = 3,
                    UserId = "adminId",
                    Name = "TestContactForm3",
                    Email = "TestEmail3",
                    Subject = "TestSubject3",
                    Message = "TestMessage3",
                    CreatedOn = DateTime.Now,
                }
            };

            var options = new DbContextOptionsBuilder<BlogAppDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactFormInMemoryDb")
                .Options;

            context = new BlogAppDbContext(options);

            context.ContactFormEntries.AddRange(contactForms);

            context.SaveChanges();

            contactService = new ContactService(context);
        }

        [Test]
        public async Task TestingSubmitContactFormMethod()
        {
            int count = 4;
            var model = new ContactViewModel()
            {
                UserId = "userId",
                Name = "TestForm",
                Email = "TestEmail",
                Subject = "TestSubject",
                Message = "TestMessage"
            };

            await contactService.SubmitContactForm(model);

            int actualCount = context.ContactFormEntries.Count();

            Assert.AreEqual(count, actualCount);
        }

        [TearDown]
        public void TearDown()
        {
            contactService = null;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
