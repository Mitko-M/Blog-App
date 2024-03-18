using BlogApp.Core.Contracts;
using BlogApp.Core.Enumerations;
using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly BlogAppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        public PostService(
            BlogAppDbContext context, 
            ICategoryService categoryService, 
            ITagService tagService)
        {
            _context = context;
            _categoryService = categoryService;
            _tagService = tagService;
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

        public async Task<AddPostFormModel> GetPostFormModel()
        {
            AddPostFormModel model = new AddPostFormModel()
            {
                Categories = await GetCategoriesWithIsSelected(),
                Tags = await GetTagsWithIsSelected()
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

        public async Task<IEnumerable<PostCategoryFormModel>> GetCategoriesWithIsSelected()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return categories.Select(c => new PostCategoryFormModel()
            {
                Id = c.Id,
                Name = c.Name,
                IsSelected = false
            })
            .ToList();
        }

        public async Task<IEnumerable<PostTagFormModel>> GetTagsWithIsSelected()
        {
            var tags = await _tagService.GetTagsAsync();

            return tags.Select(t => new PostTagFormModel()
            {
                Id = t.Id,
                Name = t.Name,
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
            var posts = await _context.Posts
                .Include(p => p.PostsCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.User)
                .ToListAsync();

            if (tagName != null)
            {
                posts = posts
                    .Where(p => p.PostsTags.Any(pt => pt.Tag.Name == tagName))
                    .ToList();
            }

            if (categoryName != null)
            {
                posts = posts
                    .Where(p => p.PostsCategories.Any(pc => pc.Category.Name == categoryName))
                    .ToList();
            }

            if (searchTerm != null)
            {
                string normalizedTerm = searchTerm.ToLower();

                posts = posts
                    .Where(p => p.Title.ToLower().Contains(normalizedTerm) ||
                        p.ShortDescription.ToLower().Contains(normalizedTerm) ||
                        p.Content.ToLower().Contains(normalizedTerm))
                    .ToList();
            }

            posts = sorting switch
            {
                PostSorting.Newest => posts
                .OrderByDescending(p => p.UpdatedOn)
                .ToList(),
                PostSorting.Oldest => posts
                .OrderBy(p => p.UpdatedOn)
                .ToList(),
                PostSorting.TitleAscending => posts
                .OrderBy(p => p.Title)
                .ToList(),
                PostSorting.TitleDescending => posts
                .OrderByDescending(p => p.Title)
                .ToList(),
                _ => posts
            };

            var categories = await _context.Categories.ToListAsync();
            var tags = await _context.Tags.ToListAsync();

            var postsToShow = posts
                .Skip((currentPage - 1) * postsPerPage)
                .Take(postsPerPage)
                .Select(p => new PostsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ShortDescription = p.ShortDescription,
                    CreatedOn = p.CreatedOn.ToString(PostDateFormat),
                    UpdatedOn = p.UpdatedOn.ToString(PostDateFormat),
                    UserId = p.UserId,
                    UserName = p.User.UserName,
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

            int allPostsCount = posts.Count;

            return new PostQueryServiceModel()
            {
                PostsCount = allPostsCount,
                Posts = postsToShow
            };
        }

        public async Task<PostQueryServiceModel> GetMinePostsAsync(
            string UserId, 
            string? tagName = null,
            string? categoryName = null,
            PostSorting sorting = PostSorting.None, 
            int currentPage = 1,
            int postsPerPage = 1, 
            string searchTerm = null)
        {
            var posts = await _context.Posts
                .Include(p => p.PostsCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.User)
                .Where(p => p.UserId == UserId)
                .ToListAsync();

            if (tagName != null)
            {
                posts = posts
                    .Where(p => p.PostsTags.Any(pt => pt.Tag.Name == tagName))
                    .ToList();
            }

            if (categoryName != null)
            {
                posts = posts
                    .Where(p => p.PostsCategories.Any(pc => pc.Category.Name == categoryName))
                    .ToList();
            }

            if (searchTerm != null)
            {
                string normalizedTerm = searchTerm.ToLower();

                posts = posts
                    .Where(p => p.Title.ToLower().Contains(normalizedTerm) ||
                        p.ShortDescription.ToLower().Contains(normalizedTerm) ||
                        p.Content.ToLower().Contains(normalizedTerm))
                    .ToList();
            }

            posts = sorting switch
            {
                PostSorting.Newest => posts
                .OrderByDescending(p => p.UpdatedOn)
                .ToList(),
                PostSorting.Oldest => posts
                .OrderBy(p => p.UpdatedOn)
                .ToList(),
                PostSorting.TitleAscending => posts
                .OrderBy(p => p.Title)
                .ToList(),
                PostSorting.TitleDescending => posts
                .OrderByDescending(p => p.Title)
                .ToList(),
                _ => posts
            };

            var categories = await _context.Categories.ToListAsync();
            var tags = await _context.Tags.ToListAsync();

            var postsToShow = posts
                .Skip((currentPage - 1) * postsPerPage)
                .Take(postsPerPage)
                .Select(p => new PostsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ShortDescription = p.ShortDescription,
                    CreatedOn = p.CreatedOn.ToString(PostDateFormat),
                    UpdatedOn = p.UpdatedOn.ToString(PostDateFormat),
                    UserId = p.UserId,
                    UserName = p.User.UserName,
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

            int allPostsCount = posts.Count;

            return new PostQueryServiceModel()
            {
                PostsCount = allPostsCount,
                Posts = postsToShow
            };
        }

        public async Task<AddPostFormModel> GetPostToEditAsync(int id)
        {
            var post = await GetPostById(id);

            if (post == null)
            {
                return null;
            }

            var categories = await GetCategoriesWithIsSelected();
            var tags = await GetTagsWithIsSelected();

            var selectedCatsIds = post.PostsCategories
                                    .Select(pc => pc.CategoryId)
                                    .ToList();

            var selectedTagsId = post.PostsTags
                                    .Select(pt => pt.TagId)
                                    .ToList();

            foreach (var cat in categories)
            {
                if (selectedCatsIds.Contains(cat.Id))
                {
                    cat.IsSelected = true;
                }
            }

            foreach (var tag in tags)
            {
                if (selectedTagsId.Contains(tag.Id))
                {
                    tag.IsSelected = true;
                }
            }

            return new AddPostFormModel()
            {
                Title = post.Title,
                Content = post.Content,
                ShortDescription = post.ShortDescription,
                UserId = post.UserId,
                Categories = categories,
                Tags = tags,
            };
        }

        public async Task UpdatePostAsync(Post post, AddPostFormModel model)
        {
            post.Title = model.Title;
            post.Content = model.Content;
            post.ShortDescription = model.ShortDescription;
            post.UpdatedOn = DateTime.Now;

            foreach (var cat in model.Categories)
            {
                if (cat.IsSelected)
                {
                    if (!(post.PostsCategories.Select(pc => pc.CategoryId).Contains(cat.Id)))
                    {
                        var postCat = new PostCategory()
                        {
                            PostId = post.Id,
                            CategoryId = cat.Id
                        };

                        post.PostsCategories.Add(postCat);
                    }
                }
                else
                {
                    if (post.PostsCategories.Select(pc => pc.CategoryId).Contains(cat.Id))
                    {
                        var postCatToRemove = post.PostsCategories.First(pc => pc.CategoryId == cat.Id);

                        post.PostsCategories.Remove(postCatToRemove);
                    }
                }
            }

            foreach (var tag in model.Tags)
            {
                if (tag.IsSelected)
                {
                    if (!(post.PostsTags.Select(pt => pt.TagId).Contains(tag.Id)))
                    {
                        var postTag = new PostTag()
                        {
                            TagId = tag.Id,
                            PostId = post.Id,
                        };

                        post.PostsTags.Add(postTag);
                    }
                }
                else
                {
                    if (post.PostsTags.Select(pt => pt.TagId).Contains(tag.Id))
                    {
                        var postTagToRemove = post.PostsTags.First(pt => pt.TagId == tag.Id);

                        post.PostsTags.Remove(postTagToRemove);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
        }
    }
}
