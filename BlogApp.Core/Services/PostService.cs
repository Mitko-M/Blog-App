using BlogApp.Core.Contracts;
using BlogApp.Core.Enumerations;
using BlogApp.Core.Models;
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

        public AddPostFormModel GetPostFormModel()
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

        public async Task<Post?> GetPostById(int id)
        {
            return await _context.Posts
                .Include(p => p.PostsCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PostQueryServiceModel> GetAllPostsAsync(
            string? tagName = null,
            string? categoryName = null,
            PostSorting sorting = PostSorting.None,
            int currentPage = 1,
            int postsPerPage = 1,
            string searchTerm = null)
        {
            var postsToShow = await _context.Posts
                .Include(p => p.PostsCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.User)
                .ToListAsync();

            if (tagName != null)
            {
                postsToShow = postsToShow
                    .Where(p => p.PostsTags.Any(pt => pt.Tag.Name == tagName))
                    .ToList();
            }

            if (categoryName != null)
            {
                postsToShow = postsToShow
                    .Where(p => p.PostsCategories.Any(pc => pc.Category.Name == categoryName))
                    .ToList();
            }

            if (searchTerm != null)
            {
                string normalizedTerm = searchTerm.ToLower();

                postsToShow = postsToShow
                    .Where(p => p.Title.ToLower().Contains(normalizedTerm) ||
                        p.ShortDescription.ToLower().Contains(normalizedTerm) ||
                        p.Content.ToLower().Contains(normalizedTerm))
                    .ToList();
            }

            postsToShow = sorting switch
            {
                PostSorting.Newest => postsToShow
                .OrderByDescending(p => p.UpdatedOn)
                .ToList(),
                PostSorting.Oldest => postsToShow
                .OrderBy(p => p.UpdatedOn)
                .ToList(),
                PostSorting.TitleAscending => postsToShow
                .OrderBy(p => p.Title)
                .ToList(),
                PostSorting.TitleDescending => postsToShow
                .OrderByDescending(p => p.Title)
                .ToList(),
                _ => postsToShow
            };

            var categories = await _context.Categories.ToListAsync();
            var tags = await _context.Tags.ToListAsync();

            var posts = postsToShow
                .Skip((currentPage - 1) * postsPerPage)
                .Take(postsPerPage)
                .Select(p => new PostsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ShortDescription = p.ShortDescription,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                    UserId = p.UserId,
                    Categories = categories
                                    .Where(c => p.PostsCategories
                                                    .Select(pc => pc.CategoryId)
                                                    .Contains(c.Id))
                                    .Select(c => c.Name)
                                    .ToList(),
                    Tags = tags
                            .Where(t => p.PostsTags
                                            .Select(pt => pt.TagId)
                                            .Contains(t.Id))
                            .Select(t => t.Name)
                            .ToList()
                })
                .ToList();

            int allPostsCount = postsToShow.Count;

            return new PostQueryServiceModel()
            {
                PostsCount = allPostsCount,
                Posts = posts
            };
        }
    }
}
