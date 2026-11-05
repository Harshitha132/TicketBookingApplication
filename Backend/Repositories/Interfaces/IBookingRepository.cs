
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int bookingId);

    Task<List<Booking>> GetAllAsync();

    Task<List<Booking>> GetByShowIdAsync(int showId);

    Task<Booking> AddAsync(Booking booking);

    Task UpdateAsync(Booking booking);
}
