using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly BlogAppDbContext _context;
        private readonly IPostService _postService;
        public CommentService(BlogAppDbContext context,
            IPostService postService)
        {
            _context = context;
            _postService = postService;
        }
        public async Task AddCommentAsync(CommentFormModel model)
        {
            var post = _postService.GetPostById(model.PostId);

            if (post == null)
            {
                throw new ArgumentException("Post doesn't exist");
            }

            var comment = new Comment()
            {
                Content = model.Content,
                UserId = model.UserId,
                PostId = model.PostId,
                CommentUploadDate = DateTime.Now
            };

            await _context.Comments.AddAsync(comment);

            await _context.SaveChangesAsync();
        }

        public async Task LikeComment(int commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment doesn't exist");
            }

            var like = new CommentLike()
            {
                CommentId = comment.Id,
                UserId = userId
            };

            comment.CommentsLikes.Add(like);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentViewModel>> LoadCommentsAsync(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = c.User.UserName,
                    PostId = c.PostId,
                    CommentUploadDate = c.CommentUploadDate,
                    CommentsLikes = c.CommentsLikes
                                        .Select(cl => new CommentLikeViewModel()
                                        {
                                            Id = cl.Id,
                                            CommentId = cl.Id,
                                            UserId = cl.UserId
                                        })
                                        .ToList()
                })
                .OrderBy( c => c.CommentUploadDate)
                .ToListAsync();

            return comments;
        }

        public async Task UnlikeComment(int commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment doesn't exist");
            }

            var commentLike = await _context.CommentsLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (commentLike == null)
            {
                throw new ArgumentException("CommentLike doesn't exist");
            }

            _context.CommentsLikes.Remove(commentLike);
        }
    }
}
