
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(int movieId);

    Task<List<Movie>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title = null);

    Task<Movie> AddAsync(Movie movie);

    Task UpdateAsync(Movie movie);

    Task DeleteAsync(Movie movie);

    Task<bool> ExistsAsync(int movieId);
}
