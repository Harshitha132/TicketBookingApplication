
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface IShowSeatRepository
{
    Task<ShowSeat?> GetByIdAsync(int showSeatId);

    Task<List<ShowSeat>> GetByShowIdAsync(int showId);

    Task<ShowSeat?> GetByShowIdAndSeatIdAsync(
        int showId,
        int seatId);

    Task AddRangeAsync(List<ShowSeat> showSeats);

    Task UpdateAsync(ShowSeat showSeat);

    Task<bool> ShowExistsAsync(int showId);

    Task<bool> SeatExistsAsync(int seatId);
}
