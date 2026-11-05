
using TicketBookingApplication.DTOs.Screens;

namespace TicketBookingApplication.Services.Interfaces;

public interface IScreenService
{
    Task<ScreenResponse?> GetByIdAsync(int screenId);

    Task<List<ScreenResponse>> GetAllAsync();

    Task<List<ScreenResponse>> GetByTheatreIdAsync(int theatreId);

    Task<ScreenResponse> CreateAsync(CreateScreenRequest request);

    Task<bool> UpdateAsync(int screenId, UpdateScreenRequest request);

    Task<bool> DeleteAsync(int screenId);
}
