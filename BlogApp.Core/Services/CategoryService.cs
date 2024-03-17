using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
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
    }
}
