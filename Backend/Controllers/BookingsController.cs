
using Microsoft.AspNetCore.Mvc;
using TicketBookingApplication.DTOs.Bookings;
using TicketBookingApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TicketBookingApplication.Controllers;

[Authorize]
[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // POST: api/bookings
    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> CreateBooking(
        [FromBody] CreateBookingRequest request)
    {
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            return Unauthorized(new
            {
                message = "Customer email not found in authentication token."
            });
        }

        
        try
        {
            var booking = await _bookingService.CreateAsync(
    request,
    customerEmail);

            return CreatedAtAction(
                nameof(GetBookingById),
                new { id = booking.BookingId },
                booking);
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

    // GET: api/bookings
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(bookings);
    }

    // GET: api/bookings/{id}
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found." });
        }

        return Ok(booking);
    }

    // GET: api/bookings/show/{showId}
    [Authorize]
    [HttpGet("show/{showId:int}")]
    public async Task<IActionResult> GetBookingsByShowId(int showId)
    {
        try
        {
            var bookings = await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // PUT: api/bookings/{id}/cancel
    [Authorize]
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        try
        {
            var booking = await _bookingService.CancelAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            return Ok(booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
