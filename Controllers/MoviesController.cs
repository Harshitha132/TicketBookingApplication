
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Movies;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movies/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);

        if (movie == null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // GET: api/movies?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? title = null)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest(new { message = "Invalid pagination values." });

        var movies = await _movieService.GetAllAsync(
            pageNumber, pageSize, title);

        return Ok(movies);
    }

    // POST: api/movies
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMovieRequest request)
    {
        var movie = await _movieService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = movie.MovieId },
            movie);
    }

    // PUT: api/movies/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMovieRequest request)
    {
        var result = await _movieService.UpdateAsync(id, request);

        if (!result)
            return NotFound(new { message = "Movie not found." });

        return NoContent();
    }

    // DELETE: api/movies/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _movieService.DeleteAsync(id);

        if (!result)
            return NotFound(new { message = "Movie not found." });

        return NoContent();
    }

    // GET: api/movies/search?title=leo
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest(new { message = "Title is required." });

        var movies = await _movieService.SearchAsync(title);

        return Ok(movies);
    }
}
