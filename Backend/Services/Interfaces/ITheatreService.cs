
using TicketBookingApplication.DTOs.Theatres;

namespace TicketBookingApplication.Services.Interfaces;

public interface ITheatreService
{
    Task<TheatreResponse?> GetByIdAsync(int theatreId);

    Task<List<TheatreResponse>> GetAllAsync();

    Task<TheatreResponse> CreateAsync(CreateTheatreRequest request);

    Task<bool> UpdateAsync(
        int theatreId,
        UpdateTheatreRequest request);

    Task<bool> DeleteAsync(int theatreId);
}
