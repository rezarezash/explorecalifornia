using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.Controllers
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
