using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        /// Returns all categories
        /// </summary>
        /// <returns>A sequence with CategoryViewModel</returns>
        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();
    }
}
