﻿using BlogApp.Core.Contracts;
using BlogApp.Core.Enumerations;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
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

            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new ArgumentException("Post wasn't added succesfully");
            }

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

            int postSaves = await _context.SaveChangesAsync();

            if (postSaves == 0)
            {
                throw new ArgumentException("Post's categories or tags were't added succesfully");
            }
        }

        public async Task<AddPostFormModel> GetPostFormModel()
        {
            AddPostFormModel model = new AddPostFormModel()
            {
                Categories = await _categoryService.GetCategoriesWithIsSelected(),
                Tags = await _tagService.GetTagsWithIsSelected()
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

        public async Task<Post?> GetPostById(int id)
        {
            return await _context.Posts
                .Include(p => p.PostsCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.PostsTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Favorites)
                .Include(p => p.LikesDislikes)
                .Include(p => p.PostReports)
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
                .OrderByDescending(p => p.CreatedOn)
                .ToList(),
                PostSorting.Oldest => posts
                .OrderBy(p => p.CreatedOn)
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
                .Where(p => p.Hidden == false)
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
                            .ToList(),
                    Hidden = p.Hidden
                })
                .ToList();

            int hidden = posts.Where(p => p.Hidden).Count();
            int allPostsCount = posts.Count - hidden;

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
                .OrderByDescending(p => p.CreatedOn)
                .ToList(),
                PostSorting.Oldest => posts
                .OrderBy(p => p.CreatedOn)
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
                            .ToList(),
                    Hidden = p.Hidden
                })
                .ToList();

            int allPostsCount = posts.Count;

            return new PostQueryServiceModel()
            {
                PostsCount = allPostsCount,
                Posts = postsToShow
            };
        }

        public async Task<AddPostFormModel> GetPostToEditAsync(int id, Post givenPost = null)
        {
            //if a post isn't given then we take it by it's id
            //i added this parameter for unit testing
            //since the relational methods such as Include and ThenInclude don't work
            //with the ef core in memory database
            if (givenPost == null)
            {
                givenPost = await GetPostById(id);
            }

            if (givenPost == null)
            {
                return null;
            }

            var categories = await _categoryService.GetCategoriesWithIsSelected();
            var tags = await _tagService.GetTagsWithIsSelected();

            var selectedCatsIds = givenPost.PostsCategories
                                    .Select(pc => pc.CategoryId)
                                    .ToList();

            var selectedTagsId = givenPost.PostsTags
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
                Title = givenPost.Title,
                Content = givenPost.Content,
                ShortDescription = givenPost.ShortDescription,
                UserId = givenPost.UserId,
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

            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new ArgumentException("Post wasn't updated succesfully");
            }
        }

        public async Task DeletePostAsync(Post post)
        {
            foreach (var comment in post.Comments)
            {
                _context.CommentsLikes.RemoveRange(comment.CommentsLikes);
            }

            await _context.SaveChangesAsync();

            _context.Comments.RemoveRange(post.Comments);
            _context.LikesDislikes.RemoveRange(post.LikesDislikes);
            _context.Favorites.RemoveRange(post.Favorites);
            _context.PostsReports.RemoveRange(post.PostReports);

            await _context.SaveChangesAsync();

            _context.Posts.Remove(post);

            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new ArgumentException("Post wasn't deleted succesfully");
            }
        }

        public PostDetailsViewModel GetPostDetailsViewModel(Post post)
        {
            var categories = post.PostsCategories
                .Select(pc => pc.Category.Name)
                .ToList();

            var tags = post.PostsTags
                .Select(pt => pt.Tag.Name)
                .ToList();

            int likes = post.LikesDislikes
                .Where(ld => ld.Liked)
                .Count();

            int dislikes = post.LikesDislikes
                .Where(ld => !ld.Liked)
                .Count();

            int favorites = post.Favorites.Count();

            PostDetailsViewModel model = new PostDetailsViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ShortDescription = post.ShortDescription,
                CreatedOn = post.CreatedOn.ToString(PostDateFormat),
                UpdatedOn = post.UpdatedOn.ToString(PostDateFormat),
                UserName = post.User.UserName,
                UserId = post.UserId,
                Categories = categories,
                Tags = tags,
                Likes = likes,
                Dislikes = dislikes,
                Favorites = favorites,
            };

            return model;
        }

        public async Task ReportPost(PostReportViewModel reportViewModel, Post givenPost = null)
        {
            //if a post isn't given then we take it by it's id
            //i added this parameter for unit testing
            //since the relational methods such as Include and ThenInclude don't work
            //with the ef core in memory database
            if (givenPost == null)
            {
                givenPost = await GetPostById(reportViewModel.PostId);
            }

            if (givenPost == null)
            {
                throw new ArgumentException($"Post with id {reportViewModel.PostId} wasn't found");
            }

            var report = new PostReport()
            {
                PostId = reportViewModel.PostId,
                UserId = reportViewModel.UserId,
                ReportContent = reportViewModel.ReportContent
            };

            givenPost.PostReports.Add(report);

            await _context.SaveChangesAsync();
        }
    }
}
