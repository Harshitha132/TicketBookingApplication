using TicketBookingApplication.DTOs.Movies;

namespace TicketBookingApplication.Services.Interfaces;

public interface IMovieService
{
    Task<MovieResponse?> GetByIdAsync(int movieId);

    Task<List<MovieResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title = null);

    Task<MovieResponse> CreateAsync(CreateMovieRequest request);

    Task<bool> UpdateAsync(
        int movieId,
        UpdateMovieRequest request);

    Task<bool> DeleteAsync(int movieId);

    Task<List<MovieResponse>> SearchAsync(string title);
}