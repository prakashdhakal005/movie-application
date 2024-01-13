using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_application.Data;
using movie_application.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace movie_application.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDbContext movieDbContext;

        public HomeController(ILogger<HomeController> logger, MovieDbContext movieDbContext)
        {
            _logger = logger;
            this.movieDbContext = movieDbContext;
        }

        

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var moviePost = await movieDbContext.MoviePosts.ToListAsync();
            return View(moviePost);
        }
        
        public IActionResult AdminLogin()
        {
            return View();

        }

        [HttpPost]
        [ActionName("AdminLogin")]
        public IActionResult AdminLogin(AdminLoginModel model)
        {
            if (IsValidAdmin(model.username,model.password))
                {
                return RedirectToAction("AdminDashbord");
                 }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid user or password");
                return View();
            }

            
        }

        private bool IsValidAdmin(string username , string password)
        {
            return username == "admin" && password == "password";
        }
       
        public IActionResult AdminDashbord()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
