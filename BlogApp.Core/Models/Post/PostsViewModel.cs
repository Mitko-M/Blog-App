namespace BlogApp.Core.Models.Post
{
    public class PostsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public string ShortDescription { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string UpdatedOn { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool Hidden { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
