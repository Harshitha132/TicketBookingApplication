
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<Screen?> GetByIdAsync(int screenId);

    Task<List<Screen>> GetAllAsync();

    Task<List<Screen>> GetByTheatreIdAsync(int theatreId);

    Task<Screen> AddAsync(Screen screen);

    Task UpdateAsync(Screen screen);

    Task DeleteAsync(Screen screen);

    Task<bool> ExistsAsync(int screenId);

    Task<bool> TheatreExistsAsync(int theatreId);
}
