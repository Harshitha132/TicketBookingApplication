
using TicketBookingApplication.DTOs.Shows;

namespace TicketBookingApplication.Services.Interfaces;

public interface IShowSeatService
{
    Task<List<ShowSeatResponse>> GetByShowIdAsync(int showId);

    Task<bool> IsSeatAvailableAsync(int showId, int seatId);

    Task GenerateShowSeatsAsync(int showId, int screenId);
}
