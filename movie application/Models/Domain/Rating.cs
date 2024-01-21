namespace movie_application.Models.Domain
{
    public class Rating
    {
        public Guid ID { get; set; }
        public string username { get; set; }
        public string MovieTitle { get; set; }
        public int rate { get; set; }
    }
}
