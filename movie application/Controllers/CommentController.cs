using Microsoft.AspNetCore.Mvc;
using movie_application.Data;
using movie_application.Models;
using Microsoft.EntityFrameworkCore;
using movie_application.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using movie_application.Areas.Identity.Data;
using Microsoft.Extensions.Primitives;
using System.Xml.Linq;
namespace movie_application.Controllers
{
   
    [Authorize]
   
    public class CommentController : Controller
    { 
       
    
        private readonly MovieDbContext movieDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public Guid ID { get; private set; }
        public string? MovieTitle { get; private set; }
        public object JsonRequestBehavior { get; private set; }

        private string? text;
        private string? username;
        private DateTime commentDate;
        private object get;

        public CommentController(MovieDbContext movieDbContext, UserManager<ApplicationUser> userManager)
        {
            this.movieDbContext = movieDbContext;
            this._userManager = userManager;
        }

       

           // public ActionResult Index()
           // {
                // Retrieve average rating from the database
                //double averageRating = 5+movieDbContext.Ratings.Select(r => r.rate).DefaultIfEmpty(0).Average();

                // Pass the average rating to the view
               // ViewBag.AverageRating = averageRating;

               // return RedirectToAction("Index","Home");
          //  }

            [HttpPost]
            public async Task <IActionResult> Rate()
            {


            // Save the rating to the database
            string MovieTitle = Request.Form["MovieTitle"];
           
            string RatingValues = Request.Form["ratingValue"];
            int intValue = int.Parse(RatingValues);
            var rating = new Rating
                { 
                
                  ID=Guid.NewGuid(),
                  rate = intValue,
                  MovieTitle=MovieTitle,
                  username = _userManager.GetUserName(this.User),

                    /*, other properties if needed */
                };

            var avgRating=await movieDbContext.Ratings.Where(r => r.MovieTitle == MovieTitle).Select(r =>r.rate).ToListAsync();
            var aRating=avgRating.Any() ? avgRating.Average() : 0;
            aRating=Math.Round(aRating,1);

            var movieDetails =await movieDbContext.MoviePosts.FirstOrDefaultAsync(r => r.MovieTitle == MovieTitle);
            if (movieDetails != null)
            {
                movieDetails.averageRating = aRating;
               await movieDbContext.SaveChangesAsync();
            }

                await movieDbContext.Ratings.AddAsync(rating);
                await movieDbContext.SaveChangesAsync();

           
            // Redirect back to the index page or wherever you want
            return RedirectToAction("Index","Home");
            }

        [HttpGet]
        public async Task <ActionResult> GetComments(string movieTitle)
        {
            // Assuming you have a MovieDetails table with  Title property
            var movieDetails =await movieDbContext.Comments.FirstOrDefaultAsync(m => m.MovieTitle == movieTitle);
                   

            if (movieDetails != null)
            {
                // Retrieve comments for the specified movie title
                var comments = await movieDbContext.Comments
                    .Where(mc => mc.MovieTitle == movieDetails.MovieTitle)
                    .ToListAsync();
                var tupleModel = new Tuple<string, List<Comment>>(movieTitle, comments);
                // Pass the comments to a partial view
                return PartialView("_CommentPartial",tupleModel);
            }
            return Json(new { success = false, message = "Movie not found" });
        }

       

        [HttpGet]
        public async Task<IActionResult> Comment(string MovieTitle)

        {
            
            var comment = await movieDbContext.Comments.ToListAsync();

            if (!string.IsNullOrEmpty(MovieTitle))
            {
                comment = comment.Where(m => m.MovieTitle.Contains(MovieTitle)).ToList();
            }
           
          
            return View(comment);
        }

        [HttpPost]
        [ActionName("AddComment")]
        public async Task<object> AddComment()

        {
          //  var addcomment = new AddCommentsModel()
          //  {
           //     MovieTitle = Request.Form["MovieTitle"].ToString(),
           //     text = Request.Form["text"].ToString()

            // };
            string Text = Request.Form["text"];
            string movieTitle = Request.Form["MovieTitle"];

            var comment = new Comment
            {
                ID = Guid.NewGuid(),
                text = Text,
                MovieTitle = movieTitle,

                username = _userManager.GetUserName(this.User),
                commentDate = DateTime.Now
             };

            await movieDbContext.Comments.AddAsync(comment);
            await movieDbContext.SaveChangesAsync();


            return RedirectToAction("Index", "Home");

        }

    }
}
