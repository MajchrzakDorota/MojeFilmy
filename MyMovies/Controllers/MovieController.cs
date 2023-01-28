using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyMovies.Entities;
using MyMovies.Interfaces;

namespace MyMovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("/movies")]
        public async Task<ActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAll();
            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }
        [HttpGet("/externalMovies")]
        public async Task<ActionResult> GetMoviesFromExternalAPI()
        {
            var movies = await _movieService.GetDataFromExternalApi();
            return Ok(movies);
        }

        [HttpGet("/movies/{id}")]
        public async Task<ActionResult> GetMovieById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive number");
            }
            var movie = await _movieService.GetMovieById(id);

            return Ok(movie);
        }

        [HttpPost("/movies")]
        public async Task<ActionResult> AddMovie([FromBody] Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("Incorrect data!");
            }

            var newMovieId = await _movieService.AddMovie(movie);

            return Created($"/movies/{newMovieId}", null);
        }

        [HttpPut("/movies/{id}")]
        public async Task<ActionResult> EditMovie([FromRoute] int id, [FromBody] Movie movieUpdates)
        {
            var movie = await _movieService.EditMovie(id, movieUpdates);

            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpDelete("/movies/{id}")]
        public async Task<ActionResult> DeleteMovie([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive number");
            }

            await _movieService.DeleteMovie(id);

            return Ok();
        }



    }
}
