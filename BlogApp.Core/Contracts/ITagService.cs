using BlogApp.Core.Models;
using BlogApp.Core.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Contracts
{
    public interface ITagService
    {
        /// <summary>
        /// Returns every tag
        /// </summary>
        /// <returns>A sequence with TagViewModel</returns>
        Task<IEnumerable<TagViewModel>> GetTagsAsync();

        /// <summary>
        /// A method for taking all tags from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostTagModel</returns>
        Task<IEnumerable<PostTagFormModel>> GetTagsWithIsSelected();
    }
}
