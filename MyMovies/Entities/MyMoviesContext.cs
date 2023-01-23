using Microsoft.EntityFrameworkCore;

namespace MyMovies.Entities
{
    public class MyMoviesContext : DbContext
    {
        public MyMoviesContext(DbContextOptions<MyMoviesContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}
