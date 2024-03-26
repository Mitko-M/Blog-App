using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data.Models;

namespace BlogApp.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserViewModel>> GetUsersAsync();
    }
}
