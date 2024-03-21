using BlogApp.Core.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Components
{
    public class SearchComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AllPostsQueryModel();

            return await Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
