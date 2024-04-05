using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly BlogAppDbContext _context;
        public UserService(BlogAppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUserWithAllDataViewModel> GetUserById(string userId)
        {
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .Select(p => new PostDetailsViewModel()
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToListAsync();

            //joining the UserRole with User and Role so i can take only the role name
            var role = await _context.UserRoles
                            .Join(_context.Users,
                                userRole => userRole.UserId,
                                user => user.Id,
                                (userRole, user) => new { UserRole = userRole, User = user })
                            .Join(_context.Roles,
                                userRoleUser => userRoleUser.UserRole.RoleId,
                                role => role.Id,
                                (userRoleUser, role) => new { UserRoleUser = userRoleUser, Role = role })
                            .Where(ur => ur.UserRoleUser.User.Id == userId)
                            .Select(ur => ur.Role.Name)
                            .FirstOrDefaultAsync();

            var user =  await _context.Users
                .Where(u  => u.Id == userId)
                .Select(u => new ApplicationUserWithAllDataViewModel()
                {
                    Id = userId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = u.Email,
                    Banned = u.Banned,
                    Role = role,
                    PostCount = posts.Count,
                    Posts = posts,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> IsUserBanned(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            //if a user wasn't found it means he is loged out
            if (user == null)
            {
                return false;
            }

            return user.Banned;
        }

        public async Task UpdateUserData(ApplicationUserViewModel model, string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User wasn't found");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            int save = await _context.SaveChangesAsync();

            if (save == 0)
            {
                throw new Exception("Database wasn't saved");
            }
        }
    }
}
