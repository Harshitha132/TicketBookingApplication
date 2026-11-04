
using TicketBookingApplication.DTOs.Bookings;

namespace TicketBookingApplication.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(
        CreateBookingRequest request,
        string customerEmail);

    Task<BookingResponse?> GetByIdAsync(int bookingId);

    Task<List<BookingResponse>> GetAllAsync();

    Task<List<BookingResponse>> GetByShowIdAsync(int showId);

    Task<BookingResponse?> CancelAsync(int bookingId);
}
