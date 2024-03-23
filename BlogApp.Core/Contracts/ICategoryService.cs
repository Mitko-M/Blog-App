using BlogApp.Core.Models;
using BlogApp.Core.Models.Post;
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

        /// <summary>
        /// A method for taking all categories from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostCategoryModel</returns>
        Task<IEnumerable<PostCategoryFormModel>> GetCategoriesWithIsSelected();
    }
}
