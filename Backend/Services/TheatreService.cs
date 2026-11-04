
using TicketBookingApplication.DTOs.Theatres;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;

    public TheatreService(ITheatreRepository theatreRepository)
    {
        _theatreRepository = theatreRepository;
    }

    public async Task<TheatreResponse?> GetByIdAsync(int theatreId)
    {
        var theatre = await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
            return null;

        return MapToResponse(theatre);
    }

    public async Task<List<TheatreResponse>> GetAllAsync()
    {
        var theatres = await _theatreRepository.GetAllAsync();

        return theatres.Select(MapToResponse).ToList();
    }

    public async Task<TheatreResponse> CreateAsync(CreateTheatreRequest request)
    {
        var theatre = new Theatre
        {
            Name = request.Name,
            Location = request.Location
        };

        var createdTheatre = await _theatreRepository.AddAsync(theatre);

        return MapToResponse(createdTheatre);
    }

    public async Task<bool> UpdateAsync(
        int theatreId,
        UpdateTheatreRequest request)
    {
        var theatre = await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
            return false;

        theatre.Name = request.Name;
        theatre.Location = request.Location;

        await _theatreRepository.UpdateAsync(theatre);

        return true;
    }

    public async Task<bool> DeleteAsync(int theatreId)
    {
        var theatre = await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
            return false;

        await _theatreRepository.DeleteAsync(theatre);

        return true;
    }

    private static TheatreResponse MapToResponse(Theatre theatre)
    {
        return new TheatreResponse
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}
