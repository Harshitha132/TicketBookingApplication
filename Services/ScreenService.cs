
using TicketBookingApplication.DTOs.Screens;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;

    public ScreenService(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    public async Task<ScreenResponse?> GetByIdAsync(int screenId)
    {
        var screen = await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
            return null;

        return MapToResponse(screen);
    }

    public async Task<List<ScreenResponse>> GetAllAsync()
    {
        var screens = await _screenRepository.GetAllAsync();

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<List<ScreenResponse>> GetByTheatreIdAsync(int theatreId)
    {
        var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<ScreenResponse> CreateAsync(CreateScreenRequest request)
    {
        // Check whether the theatre exists
        var theatreExists = await _screenRepository
            .TheatreExistsAsync(request.TheatreId);

        if (!theatreExists)
            throw new ArgumentException("The specified theatre does not exist.");

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = request.ScreenName,
            TotalSeats = request.TotalSeats
        };

        var createdScreen = await _screenRepository.AddAsync(screen);

        return MapToResponse(createdScreen);
    }

    public async Task<bool> UpdateAsync(
        int screenId,
        UpdateScreenRequest request)
    {
        var screen = await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
            return false;

        screen.ScreenName = request.ScreenName;
        screen.TotalSeats = request.TotalSeats;

        await _screenRepository.UpdateAsync(screen);

        return true;
    }

    public async Task<bool> DeleteAsync(int screenId)
    {
        var screen = await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
            return false;

        await _screenRepository.DeleteAsync(screen);

        return true;
    }

    private static ScreenResponse MapToResponse(Screen screen)
    {
        return new ScreenResponse
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName,
            TotalSeats = screen.TotalSeats
        };
    }
}
