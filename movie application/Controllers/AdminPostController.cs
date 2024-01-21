using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_application.Data;
using movie_application.Models;
using movie_application.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace movie_application.Controllers
{
   // [Authorize]
    public class AdminPostController : Controller
    {
        private readonly MovieDbContext movieDbContext;

        public AdminPostController(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> PostView()
        {
            var moviePost =await movieDbContext.MoviePosts.ToListAsync();
          
            return View(moviePost);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddPostViewModel addPostRequest)
        {
            var moviePost = new MoviePost()
            {
                ID = Guid.NewGuid(),
                MovieTitle = addPostRequest.MovieTitle,
                MovieDescription = addPostRequest.MovieDescription,
                MovieImageUrl = addPostRequest.MovieImageUrl,
                MovieReleasedDate = addPostRequest.MovieReleasedDate,
                MovieVideoUrl = addPostRequest.MovieVideoUrl,
                PublishedDate = addPostRequest.PublishedDate


            };
            await movieDbContext.MoviePosts.AddAsync(moviePost);
            await movieDbContext.SaveChangesAsync();
            return View("Add");

        }

        [HttpGet]
        public async Task<IActionResult> ViewAsync(Guid id)
        {
            var moviePost = movieDbContext.MoviePosts.First(x => x.ID == id);
            if (moviePost != null)
            {
                var viewModel = new PostUpdateViewModel()
                {
                    ID = moviePost.ID,
                    MovieTitle = moviePost.MovieTitle,
                    MovieDescription = moviePost.MovieDescription,
                    MovieImageUrl = moviePost.MovieImageUrl,
                    MovieReleasedDate = moviePost.MovieReleasedDate,
                    MovieVideoUrl = moviePost.MovieVideoUrl,
                    PublishedDate = moviePost.PublishedDate
                };

                return await Task.Run(() => View("View", viewModel));
            }
            return View("View");
        }

        [HttpPost]
        public async Task<IActionResult> View(PostUpdateViewModel model)
        {
            var moviePost = await movieDbContext.MoviePosts.FindAsync(model.ID);
            if (moviePost != null)
            {
                moviePost.MovieTitle = model.MovieTitle;
                moviePost.MovieDescription = model.MovieDescription;
                moviePost.MovieImageUrl = model.MovieImageUrl;
                moviePost.MovieVideoUrl = model.MovieVideoUrl;
                moviePost.MovieReleasedDate = model.MovieReleasedDate;
                moviePost.PublishedDate = model.PublishedDate;

                await movieDbContext.SaveChangesAsync();
                return RedirectToAction("PostView");
            }
            return RedirectToAction("PostView");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PostUpdateViewModel model)
        {
            var moviePost = await movieDbContext.MoviePosts.FindAsync(model.ID);
            if (moviePost != null) 
            {
                movieDbContext.MoviePosts.Remove(moviePost);
                await movieDbContext.SaveChangesAsync();
                return RedirectToAction("PostView");
            }
            return RedirectToAction("PostView");

        }
    }
}

