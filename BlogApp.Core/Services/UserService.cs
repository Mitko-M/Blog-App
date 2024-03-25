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
        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
