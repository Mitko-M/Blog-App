using BlogApp.Core.Models.Contact;

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
