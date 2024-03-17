namespace BlogApp.Core.Models.Post
{
    public class PostsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public string ShortDescription { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserId { get; set; } = string.Empty;

        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
