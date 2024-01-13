using Microsoft.AspNetCore.Mvc;

namespace movie_application.Controllers
{
    public class AdminTagController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
