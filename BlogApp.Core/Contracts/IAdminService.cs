using BlogApp.Core.Models;
using BlogApp.Core.Models.Contact;
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

        /// <summary>
        /// Finds and returns a report by it's id
        /// </summary>
        /// <returns></returns>
        Task<PostReportsAdminViewModel> GetReportById(int id);

        /// <summary>
        /// Deletes a report with the given id
        /// </summary>
        /// <param name="id">The Report's Id</param>
        /// <returns></returns>
        Task DeleteReport(int id);

        /// <summary>
        /// Adds a warning to a user form a reported post then hides the post and if the user reaches 3 warnings
        /// he/she is banned
        /// </summary>
        /// <param name="reportId">The report'identifier to be deleted after warning</param>
        /// <param name="postId">The reported post's identifier for the post to be hiden after a warning</param>
        /// <param name="userId">The user identifier to be warned and potentially baned if he must</param>
        /// <returns></returns>
        Task WarnApplicationUser(int reportId, int postId, string userId);

        /// <summary>
        /// Returns all submitted contact forms converted to a view model
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContactAdminViewModel>> GetAllContactFormsAsync();

        /// <summary>
        /// Finds and returns a contact form entry by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ContactAdminViewModel> GetContactFormById(int id);

        /// <summary>
        /// Deletes a contact form entry with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteContactFormEntry(int id);

        /// <summary>
        /// Gets the user by username and gives the admin the rights to manage him/her
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApplicationUserWithAllDataViewModel> ManageUserByUserName(string userName);
    }
}
