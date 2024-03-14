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
                if (category.IsSelected)
                {
                    await _context.PostsCategories.AddAsync(new PostCategory()
                    {
                        PostId = postToAdd.Id,
                        CategoryId = category.Id
                    });
                }
            }

            foreach (var tag in model.Tags)
            {
                if (tag.IsSelected)
                {
                    await _context.PostsTags.AddAsync(new PostTag()
                    {
                        PostId = postToAdd.Id,
                        TagId = tag.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
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

        public AddPostFormModel GetPostFormModelAsync()
        {
            AddPostFormModel model = new AddPostFormModel()
            {
                Categories = GetCategories(),
                Tags = GetTags()
            };

            return model;
        }

        public List<int> RequestSelectionToList(string values)
        {
            List<int> ret = new List<int>();

            List<string> selected = values.Split(',').ToList();

            int value = 0;

            foreach (string item in selected)
            {
                if (int.TryParse(item, out value))
                {
                    ret.Add(value);
                }
            }

            return ret;
        }

        public List<PostCategoryModel> GetCategories()
        {
            return _context.Categories
                .Select(c => new PostCategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = false
                })
                .ToList();
        }

        public List<PostTagModel> GetTags()
        {
            return _context.Tags
                .Select(c => new PostTagModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = false
                })
                .ToList();
        }
    }
}
