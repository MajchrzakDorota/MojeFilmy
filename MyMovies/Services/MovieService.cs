using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyMovies.Entities;
using MyMovies.Interfaces;

namespace MyMovies.Services
{
    public class MovieService : IMovieService
    {
        private readonly MyMoviesContext _context;
        private readonly ILogger<MovieService> _logger;

        public MovieService(MyMoviesContext context, ILogger<MovieService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Movie>> GetAll()
        {
            var movies = _context.Movies.ToList();

            if(movies == null)
            {
                throw new NullReferenceException("Any movie not found!");
            }
            return await Task.FromResult(movies);
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if(movie == null)
            {
                throw new NullReferenceException($"Movie with Id: {id} not found!");
            }
            return await Task.FromResult(movie);
        }

        public async Task<int> AddMovie(Movie movie)
        {
            var newMovie = new Movie()
            {
                Id = movie.Id,
                Title = movie.Title,
                Director= movie.Director,
                Year= movie.Year,
                Rate= movie.Rate
            };

            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();

            return newMovie.Id;
        }

        public async Task<Movie> EditMovie(int id, JsonPatchDocument<Movie> movieUpdates)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                throw new NullReferenceException($"Movie with Id: {id} not found!");
            }

            movieUpdates.ApplyTo(movie);

            await _context.SaveChangesAsync();

            return movie;

        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if(movie == null)
            {
                throw new NullReferenceException($"Movie with Id: {id} not found!");
            }

            _context.Movies.Remove(movie);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return true;
        } 
    }
}
