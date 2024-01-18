using Microsoft.EntityFrameworkCore;
using movie_application.Models.Domain;

namespace movie_application.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }
        public DbSet<MoviePost> MoviePosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public object MoviePost { get; internal set; }
    }
}
