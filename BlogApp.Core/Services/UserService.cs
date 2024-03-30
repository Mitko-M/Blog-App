using BlogApp.Core.Contracts;
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
    }
}
