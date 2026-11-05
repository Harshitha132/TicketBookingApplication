
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface IShowRepository
{
    Task<Show?> GetByIdAsync(int showId);

    Task<List<Show>> GetAllAsync(
        DateTime? date = null,
        int? movieId = null,
        int? theatreId = null);

    Task<List<Show>> GetByMovieIdAsync(int movieId);

    Task<List<Show>> GetByTheatreIdAsync(int theatreId);

    Task<Show> AddAsync(Show show);

    Task UpdateAsync(Show show);

    Task DeleteAsync(Show show);

    Task<bool> MovieExistsAsync(int movieId);

    Task<bool> ScreenExistsAsync(int screenId);

    Task<bool> HasOverlappingShowAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null);
}
