using ExploreCalifornia.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.ViewComponents
{
    
    public class MontlhySpecialsViewComponent : ViewComponent
    {
        private readonly BlogDbContext _dbContext;

        public MontlhySpecialsViewComponent(BlogDbContext blogDataContext)
        {
            _dbContext = blogDataContext;
        }
        
        public IViewComponentResult Invoke()
        {           
            return View(_dbContext.MonthlySpecials);
        }
    }
}
