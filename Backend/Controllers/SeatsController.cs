
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Seats;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Controllers;

[ApiController]
[Route("api/seats")]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // GET: api/seats/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);

        if (seat == null)
            return NotFound(new { message = "Seat not found." });

        return Ok(seat);
    }

    // GET: api/seats/screen/{screenId}
    [HttpGet("screen/{screenId:int}")]
    public async Task<IActionResult> GetByScreenId(int screenId)
    {
        try
        {
            var seats = await _seatService.GetAllByScreenIdAsync(screenId);
            return Ok(seats);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/seats
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seat.SeatId },
                seat);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // POST: api/seats/bulk
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate(
        [FromBody] BulkCreateSeatsRequest request)
    {
        try
        {
            var seats = await _seatService.BulkCreateAsync(request);
            return StatusCode(StatusCodes.Status201Created, seats);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/seats/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSeatRequest request)
    {
        try
        {
            var updated = await _seatService.UpdateAsync(id, request);

            if (!updated)
                return NotFound(new { message = "Seat not found." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/seats/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _seatService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Seat not found." });

        return NoContent();
    }
}
