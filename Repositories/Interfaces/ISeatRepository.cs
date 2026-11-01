
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface ISeatRepository
{
    Task<Seat?> GetByIdAsync(int seatId);

    Task<List<Seat>> GetAllByScreenIdAsync(int screenId);

    Task<Seat> AddAsync(Seat seat);

    Task AddRangeAsync(List<Seat> seats);

    Task UpdateAsync(Seat seat);

    Task DeleteAsync(Seat seat);

    Task<bool> ExistsAsync(int seatId);

    Task<bool> ScreenExistsAsync(int screenId);

    Task<bool> SeatExistsAsync(
        int screenId,
        string rowName,
        int seatNumber);
}
