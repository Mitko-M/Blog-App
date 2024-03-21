using BlogApp.Core.Models.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Components
{
    public class CommentFormComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = new CommentFormModel();
            model.PostId = id;

            return await Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
