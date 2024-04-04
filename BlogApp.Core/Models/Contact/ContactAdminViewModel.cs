namespace BlogApp.Core.Models.Contact
{
    public class ContactAdminViewModel : ContactViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string CreatedOn { get; set; }
    }
}
