using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data;
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

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAdminsAsync()
        {
            var users = await GetUsersOnRoleNameAsync("Admin");

            return users;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync()
        {
            var admins = await GetAdminsAsync();
            var users = await GetUsersOnRoleNameAsync("User");
            var allUsers = admins.Concat(users).ToList();

            return allUsers;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersOnRoleNameAsync(string roleName)
        {
            //joining the AspNetUser with AspNetRoles and AspNetUserRoles
            //and then we take only the needed values
            var joinedUserRoles = _context.UserRoles
                            .Join(_context.Users,
                                userRole => userRole.UserId,
                                user => user.Id,
                                (userRole, user) => new { UserRole = userRole, User = user })
                            .Join(_context.Roles,
                                userRoleUser => userRoleUser.UserRole.RoleId,
                                role => role.Id,
                                (userRoleUser, role) => new { UserRoleUser = userRoleUser, Role = role })
                            .Select(x => new
                            {
                                UserId = x.UserRoleUser.User.Id,
                                RoleId = x.Role.Id,
                                RoleName = x.Role.Name
                            })
                            .ToList();

            //we take all users
            var users = await _context.ApplicationUsers.ToListAsync();

            //we take all roles
            var roles = await _context.Roles.ToArrayAsync();

            //we take all posts
            var posts = await _context.Posts.ToListAsync();

            //here we take the role Id by comparing the given role name
            string roleId = roles.FirstOrDefault(u => u.NormalizedName == roleName.ToUpper()).Id;


            //defining model
            var model = users.Select(u => new ApplicationUserViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                PostCount = posts.Where(p => p.UserId == u.Id).Count(),
                Role = joinedUserRoles.Where(jt => jt.RoleId == roleId && jt.UserId == u.Id).Select(jt => jt.RoleName).FirstOrDefault()
            })
                .ToList();

            //taking the users only with the given role
            model = model.Where(u => u.Role == roleName).ToList();

            return model;
        }
    }
}
