
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface ITheatreRepository
{
    Task<Theatre?> GetByIdAsync(int theatreId);

    Task<List<Theatre>> GetAllAsync();

    Task<Theatre> AddAsync(Theatre theatre);

    Task UpdateAsync(Theatre theatre);

    Task DeleteAsync(Theatre theatre);

    Task<bool> ExistsAsync(int theatreId);
}
