using movie_application.Models.Domain;

namespace movie_application.Models
{
    public class IndexViewModel
    {
        public List<MoviePost> movieData { get; set; }
        public Rating ratingData { get; set; }
    }
}
