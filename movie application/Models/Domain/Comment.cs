namespace movie_application.Models.Domain
{
    public class Comment
    {
        public int ID { get; set; }
        public string MovieTitle { get; set; }
        public string username { get; set; }
        public string text { get; set; }
        public DateTime commentDate { get; set; }
    }
}
