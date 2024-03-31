using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly BlogAppDbContext _context;
        public ContactService(BlogAppDbContext context)
        {
            _context = context;
        }
        public async Task SubmitContactForm(ContactViewModel model)
        {
            var contact = new ContactFormEntry()
            {
                UserId = model.UserId,
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                CreatedOn = DateTime.Now,
            };

            await _context.ContactFormEntries.AddAsync(contact);


            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new ArgumentException("Contact form wasn't saved");
            }
        }
    }
}
