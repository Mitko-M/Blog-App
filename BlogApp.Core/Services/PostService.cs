using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    //TODO: write business logic
    public class PostService : IPostService
    {
        private readonly BlogAppDbContext _context;
        public PostService(BlogAppDbContext context)
        {
            _context = context;
        }
        public Task AddPostAsync(AddPostViewModel postModel, string userId)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(Post post)
        {
            throw new NotImplementedException();
        }

        public Task<PostViewModel> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AllPostsViewModel>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Select(p => new AllPostsViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ShortDescription = p.ShortDescription,
                    UserId = p.UserId,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                })
                .ToListAsync();
        }

        public Task UpdatePostAsync(AddPostViewModel postModel, Post postToEdit)
        {
            throw new NotImplementedException();
        }
    }
}
