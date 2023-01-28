using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyMovies.Entities;
using MyMovies.Interfaces;
using Newtonsoft.Json;
using System.Linq;

namespace MyMovies.Services
{
    public class MovieService : IMovieService
    {
        private readonly MyMoviesContext _context;
        private readonly ILogger<MovieService> _logger;
        private readonly string _path = "https://filmy.programdemo.pl/MyMovies";

        public MovieService(MyMoviesContext context, ILogger<MovieService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Movie>> GetAll()
        {
            try
            {
                var movies = _context.Movies.ToList();

                if (movies == null)
                {
                    throw new NullReferenceException("Any movie not found!");
                }
                return await Task.FromResult(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<Movie>> GetDataFromExternalApi()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(_path);
                var result = await response.Content.ReadAsStringAsync();

                var allMovies = await GetAll();

                var httpMovies = JsonConvert.DeserializeObject<List<Movie>>(result).Select(m =>
                {
                    m.Id = 0;
                    m.Rate = m.Rate;
                    m.Title = m.Title;
                    m.Director = m.Director;
                    m.Year = m.Year;
                    return m;
                }).ToList();

                var finalResult = new List<Movie>();

                foreach (var movie in httpMovies)
                {
                    var index = allMovies.FindIndex(m => m.Title == movie.Title);

                    if (index < 0)
                    {
                        finalResult.Add(movie);
                    }
                }

                _context.Movies.AddRange(finalResult);
                await _context.SaveChangesAsync();

                return finalResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Movie> GetMovieById(int id)
        {
            try
            {
                var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

                if (movie == null)
                {
                    throw new NullReferenceException($"Movie with Id: {id} not found!");
                }

                return await Task.FromResult(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> AddMovie(Movie movie)
        {
            try
            {
                var newMovie = new Movie()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Director = movie.Director,
                    Year = movie.Year,
                    Rate = movie.Rate
                };

                _context.Movies.Add(newMovie);
                await _context.SaveChangesAsync();

                return newMovie.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Movie> EditMovie(int id, Movie movieUpdates)
        {
            try
            {
                var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

                if (movie == null)
                {
                    throw new NullReferenceException($"Movie with Id: {id} not found!");
                }

                movie.Director = movieUpdates.Director;
                movie.Title = movieUpdates.Title;
                movie.Year = movieUpdates.Year;
                movie.Rate = movieUpdates.Rate;

                _context.Update(movie);

                await _context.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteMovie(int id)
        {
            try
            {
                var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

                if (movie == null)
                {
                    throw new NullReferenceException($"Movie with Id: {id} not found!");
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
