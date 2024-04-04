namespace BlogApp.Core.Models.Identity
{
    public class ApplicationUserWithAllDataViewModel : ApplicationUserViewModel
    {
        //TODO: when a search by username is added
        //add the option to see the user's posts
        public bool Banned { get; set; }
        public int WarningsCount { get; set; }
    }
}
