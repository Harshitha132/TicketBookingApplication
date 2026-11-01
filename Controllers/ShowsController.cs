
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Shows;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Controllers;

[ApiController]
[Route("api/shows")]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;
    private readonly IShowSeatService _showSeatService;

    // Constructor Injection
    public ShowsController(
        IShowService showService,
        IShowSeatService showSeatService)
    {
        _showService = showService;
        _showSeatService = showSeatService;
    }

    // GET: api/shows
    // Optional filters: date, movieId, theatreId
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] int? movieId,
        [FromQuery] int? theatreId)
    {
        var shows = await _showService.GetAllAsync(
            date, movieId, theatreId);

        return Ok(shows);
    }

    // GET: api/shows/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _showService.GetByIdAsync(id);

        if (show == null)
        {
            return NotFound(new
            {
                message = "Show not found."
            });
        }

        return Ok(show);
    }

    // GET: api/shows/movie/2
    [HttpGet("movie/{movieId:int}")]
    public async Task<IActionResult> GetByMovieId(int movieId)
    {
        var shows = await _showService.GetByMovieIdAsync(movieId);

        return Ok(shows);
    }

    // GET: api/shows/theatre/1
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetByTheatreId(int theatreId)
    {
        var shows = await _showService.GetByTheatreIdAsync(theatreId);

        return Ok(shows);
    }

    // GET: api/shows/5/seats
    [HttpGet("{showId:int}/seats")]
    public async Task<IActionResult> GetShowSeats(int showId)
    {
        try
        {
            var seats = await _showSeatService.GetByShowIdAsync(showId);

            return Ok(seats);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    // POST: api/shows
    [HttpPost]
    public async Task<IActionResult> Create(CreateShowRequest request)
    {
        try
        {
            var show = await _showService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = show.ShowId },
                show);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: api/shows/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateShowRequest request)
    {
        try
        {
            bool updated = await _showService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Show not found."
                });
            }

            return Ok(new
            {
                message = "Show updated successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // DELETE: api/shows/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _showService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Show not found."
            });
        }

        return Ok(new
        {
            message = "Show deleted successfully."
        });
    }
}
