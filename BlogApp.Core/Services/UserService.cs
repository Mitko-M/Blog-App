using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly BlogAppDbContext _context;
        public UserService(BlogAppDbContext context)
        {
            _context = context;
        }
        //t'va neshto e maloumno
        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersAsync()
        {
            var users = await _context.ApplicationUsers.ToListAsync();

            var userRoles = await _context.UserRoles.ToArrayAsync();

            var roles = await _context.Roles.ToArrayAsync();

            var posts = await _context.Posts.ToListAsync();

            var model = users.Select(u => new ApplicationUserViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                PostCount = posts.Where(p => p.UserId == u.Id).Count(),
                Role = roles
                        .Where(r => userRoles.FirstOrDefault(ur => ur.RoleId == r.Id).UserId == u.Id)
                        .Select(r => r.Name)
                        .FirstOrDefault()
            })
                .ToList();

            return model;
        }
    }
}
