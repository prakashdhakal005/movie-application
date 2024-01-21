using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_application.Data;
using movie_application.Models;
using movie_application.Models.Domain;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace movie_application.Controllers
{
   // [Authorize]
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

        public async Task<IActionResult> Index(string searchString)
        {
             
            var moviePost = await movieDbContext.MoviePosts.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                moviePost = moviePost.Where(m => m.MovieTitle.Contains(searchString)).ToList();
            }
            var Rating=  movieDbContext.Ratings.FirstOrDefault();

            var viewData = new IndexViewModel
            {
                movieData=moviePost,
                ratingData=Rating

            };

            return View(viewData);
        }

       public IActionResult AdminRegister()
        {
            return View();


        }

        [HttpPost]
        [ActionName("Admin")]
        public async Task <IActionResult> Admin(AdminRegisterModel addRequest)
        {
            var admin = new Admin()
            {
                ID = Guid.NewGuid(),
                FirstName = addRequest.FirstName,
                LastName = addRequest.LastName,
                Email = addRequest.Email,
                Password = addRequest.Password,
                ConfirmPassword = addRequest.ConfirmPassword
                


            };
            await movieDbContext.Admins.AddAsync(admin);
            await movieDbContext.SaveChangesAsync();

            return RedirectToAction("AdminRegister");
        }    

    public IActionResult AdminLogin()
        {
            return View();

        }

      /*  [HttpPost]
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
        }*/

        [HttpPost]
        [ActionName("AdminLogin")]
        public async Task<ActionResult> AdminLogin(AdminLoginModel user)
        {
            if (ModelState.IsValid)
            {
                // Retrieve user from the database
                var dbUser =await movieDbContext.Admins
                    .FirstOrDefaultAsync(u => u.Email == user.username && u.Password == user.password);

                if (dbUser != null)
                {
                    // Authentication successful
                    // Redirect to a secure page or perform other actions
                    return RedirectToAction("AdminDashbord", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            // If the model is not valid or authentication fails, return to the login page
            return View();
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
