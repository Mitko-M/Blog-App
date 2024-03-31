using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Contracts
{
    public interface IContactService
    {
        /// <summary>
        /// Adds a ContactFormEntry in the database most probably because a user was banned
        /// </summary>
        /// <param name="model">The contact form data</param>
        /// <returns></returns>
        Task SubmitContactForm(ContactViewModel model);
    }
}
