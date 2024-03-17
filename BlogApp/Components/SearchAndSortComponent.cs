using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Components
{
    public class SearchAndSortComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
