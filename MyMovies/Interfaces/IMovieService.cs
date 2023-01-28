using Microsoft.AspNetCore.JsonPatch;
using MyMovies.Entities;

namespace MyMovies.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAll();
        Task<Movie> GetMovieById(int id);
        Task<List<Movie>> GetDataFromExternalApi();
        Task<int> AddMovie(Movie movie);
        Task<Movie> EditMovie(int id, Movie movieUpdates);
        Task<bool> DeleteMovie(int id);
    }
}
