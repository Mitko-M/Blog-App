using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    public class TagService : ITagService
    {
        private readonly BlogAppDbContext _context;
        public TagService(BlogAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TagViewModel>> GetTagsAsync()
        {
            return await _context.Tags.Select(t => new TagViewModel()
            {
                Id = t.Id,
                Name = t.Name
            })
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
