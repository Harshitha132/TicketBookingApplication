
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Screens;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/screens")]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // GET: api/screens
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var screens = await _screenService.GetAllAsync();

        return Ok(screens);
    }

    // GET: api/screens/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var screen = await _screenService.GetByIdAsync(id);

        if (screen == null)
            return NotFound(new { message = "Screen not found." });

        return Ok(screen);
    }

    // GET: api/screens/theatre/1
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetByTheatreId(int theatreId)
    {
        var screens = await _screenService.GetByTheatreIdAsync(theatreId);

        return Ok(screens);
    }

    // POST: api/screens
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateScreenRequest request)
    {
        try
        {
            var screen = await _screenService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/screens/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateScreenRequest request)
    {
        var updated = await _screenService.UpdateAsync(id, request);

        if (!updated)
            return NotFound(new { message = "Screen not found." });

        return NoContent();
    }

    // DELETE: api/screens/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _screenService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Screen not found." });

        return NoContent();
    }
}
