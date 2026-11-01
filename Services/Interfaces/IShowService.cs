
using TicketBookingApplication.DTOs.Shows;

namespace TicketBookingApplication.Services.Interfaces;

public interface IShowService
{
    Task<ShowResponse?> GetByIdAsync(int showId);

    Task<List<ShowResponse>> GetAllAsync(
        DateTime? date = null,
        int? movieId = null,
        int? theatreId = null);

    Task<List<ShowResponse>> GetByMovieIdAsync(int movieId);

    Task<List<ShowResponse>> GetByTheatreIdAsync(int theatreId);

    Task<ShowResponse> CreateAsync(CreateShowRequest request);

    Task<bool> UpdateAsync(int showId, UpdateShowRequest request);

    Task<bool> DeleteAsync(int showId);
}
