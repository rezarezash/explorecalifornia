using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExploreCalifornia.ViewComponents
{
    public class ExploreCaliforniaLabelViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string title)
        {
            return Task.FromResult<IViewComponentResult>(View("Default", title));
        }
    }
}
