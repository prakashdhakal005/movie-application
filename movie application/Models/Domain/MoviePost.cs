namespace movie_application.Models.Domain
{
    public class MoviePost
    {
        public Guid ID { get; set; }
        public string MovieTitle { get; set; }

        public DateTime MovieReleasedDate { get; set; }

        public string MovieDescription { get; set;}

        public string MovieImageUrl { get; set;}

        public string MovieVideoUrl { get; set; }

        public DateTime PublishedDate { get; set; }


       


    }
}
