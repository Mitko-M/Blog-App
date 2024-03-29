using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data.Models;

namespace BlogApp.Core.Contracts
{
    public interface IAdminService
    {
        /// <summary>
        /// Gets users based on the given role name
        /// </summary>
        /// <param name="roleName">User role's name</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUserViewModel>> GetUsersOnRoleNameAsync(string roleName);

        /// <summary>
        /// Gets all application users that are in the Admin role
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUserViewModel>> GetAdminsAsync();

        /// <summary>
        /// Gets all application users that are in the User role
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUserViewModel>> GetUsersAsync();

        /// <summary>
        /// Returning all application users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync();

        /// <summary>
        /// Returns all reports converted to a view model
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PostReportsAdminViewModel>> GetAllReportsAsync();
    }
}
