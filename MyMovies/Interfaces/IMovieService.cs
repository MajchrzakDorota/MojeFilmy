using Microsoft.AspNetCore.JsonPatch;
using MyMovies.Entities;

namespace MyMovies.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAll();
        Task<Movie> GetMovieById(int id);
        Task<int> AddMovie(Movie movie);
        Task<Movie> EditMovie(int id, JsonPatchDocument<Movie> movieUpdates);
        Task<bool> DeleteMovie(int id);
    }
}
