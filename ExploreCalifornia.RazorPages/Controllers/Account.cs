using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.RazorPages.Controllers
{
    [Route("Account")]
    public class Account : Controller
    {
        [HttpGet("rclview")]
        public IActionResult RCL()
        {
            return View();
        }
    }
}
