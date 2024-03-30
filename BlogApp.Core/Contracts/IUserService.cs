using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Contracts
{
    public interface IUserService
    {
        /// <summary>
        /// Returns a boolean which indeicates whether a user is banned or not
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        Task<bool> IsUserBanned(string userId);
    }
}
