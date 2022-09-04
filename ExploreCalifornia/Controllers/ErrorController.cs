using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.Controllers
{
    public class ExceptionController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Exception()
        {
            var exceptionMessage = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error.Message;
            return new ProblemDetails
        }
    }
}
