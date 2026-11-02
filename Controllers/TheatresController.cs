
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Theatres;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/theatres")]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    // GET: api/theatres
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var theatres = await _theatreService.GetAllAsync();

        return Ok(theatres);
    }

    // GET: api/theatres/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var theatre = await _theatreService.GetByIdAsync(id);

        if (theatre == null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    // POST: api/theatres
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTheatreRequest request)
    {
        var theatre = await _theatreService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = theatre.TheatreId },
            theatre);
    }

    // PUT: api/theatres/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTheatreRequest request)
    {
        var updated = await _theatreService.UpdateAsync(id, request);

        if (!updated)
            return NotFound(new { message = "Theatre not found." });

        return NoContent();
    }

    // DELETE: api/theatres/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _theatreService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Theatre not found." });

        return NoContent();
    }
}
