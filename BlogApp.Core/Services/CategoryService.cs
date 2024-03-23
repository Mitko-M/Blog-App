using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogAppDbContext _context;

        public CategoryService(BlogAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            return await _context.Categories.Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<PostCategoryFormModel>> GetCategoriesWithIsSelected()
        {
            var categories = await GetCategoriesAsync();

            return categories.Select(c => new PostCategoryFormModel()
            {
                Id = c.Id,
                Name = c.Name,
                IsSelected = false
            })
            .ToList();
        }
    }
}
