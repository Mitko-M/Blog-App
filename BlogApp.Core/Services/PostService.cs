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
        public async Task AddPostAsync(AddPostFormModel model, string userId)
        {
            Post postToAdd = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                ShortDescription = model.ShortDescription,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserId = userId
            };

            await _context.Posts.AddAsync(postToAdd);

            await _context.SaveChangesAsync();

            foreach (var category in model.Categories)
            {
                await _context.PostsCategories.AddAsync(new PostCategory()
                {
                    PostId = postToAdd.Id,
                    CategoryId = category.Id
                });
            }

            await _context.SaveChangesAsync();
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

        public Task UpdatePostAsync(AddPostFormModel postModel, Post postToEdit)
        {
            throw new NotImplementedException();
        }
    }
}
