namespace BlogApp.Core.Models.Post
{
    public static class PostModelExtension
    {
        public static string GetPostTitleInformation(this PostsViewModel post)
        {
            return post.Title.Replace(" ", "-");
        }
    }
}
