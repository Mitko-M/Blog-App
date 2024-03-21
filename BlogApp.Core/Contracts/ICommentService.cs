using BlogApp.Core.Models.Comment;

namespace BlogApp.Core.Contracts
{
    public interface ICommentService
    {
        /// <summary>
        /// Adding a comment to a post asynchroniously
        /// </summary>
        /// <param name="model">The comment inputed through a form</param>
        /// <returns></returns>
        Task AddCommentAsync(CommentFormModel model);

        /// <summary>
        /// Gets all comments from the database and converts them to modol
        /// </summary>
        /// <param name="postId">The post identifier</param>
        /// <returns></returns>
        Task<IEnumerable<CommentViewModel>> LoadCommentsAsync(int postId);

        /// <summary>
        /// Adding a like to comment
        /// </summary>
        /// <param name="commentId">Comment identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task LikeComment(int commentId, string userId);

        /// <summary>
        /// Removing a like from a comment
        /// </summary>
        /// <param name="commentId">Comment identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task UnlikeComment(int commentId, string userId);
    }
}
