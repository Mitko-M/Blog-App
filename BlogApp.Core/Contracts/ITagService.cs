using BlogApp.Core.Models;
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
    }
}
