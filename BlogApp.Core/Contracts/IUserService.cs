using BlogApp.Infrastructure.Data.Models;

namespace BlogApp.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
    }
}
