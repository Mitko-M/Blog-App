namespace BlogApp.Core.Models.Post
{
    public class AllPostsVideModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public string ShortDescription { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserId { get; set; } = string.Empty;

        //add categories to be showed when viewing all posts
    }
}
