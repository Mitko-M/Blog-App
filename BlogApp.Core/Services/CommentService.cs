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
        public CommentService(BlogAppDbContext context)
        {
            _context = context;
        }
        public async Task AddCommentAsync(CommentFormModel model)
        {
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
                    UserId = c.UserId,
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
                .ToListAsync();

            return comments;
        }
    }
}
