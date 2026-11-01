
using TicketBookingApplication.DTOs.Shows;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _showRepository;

    public ShowService(IShowRepository showRepository)
    {
        _showRepository = showRepository;
    }

    public async Task<ShowResponse?> GetByIdAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        return show == null ? null : MapToResponse(show);
    }

    public async Task<List<ShowResponse>> GetAllAsync(
        DateTime? date = null,
        int? movieId = null,
        int? theatreId = null)
    {
        var shows = await _showRepository.GetAllAsync(
            date, movieId, theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowResponse>> GetByMovieIdAsync(int movieId)
    {
        var shows = await _showRepository.GetByMovieIdAsync(movieId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowResponse>> GetByTheatreIdAsync(int theatreId)
    {
        var shows = await _showRepository.GetByTheatreIdAsync(theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
    {
        ValidateShowDetails(
            request.StartTime,
            request.EndTime,
            request.TicketPrice);

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
        {
            throw new ArgumentException("Movie not found.");
        }

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
        {
            throw new ArgumentException("Screen not found.");
        }

        bool hasOverlap = await _showRepository.HasOverlappingShowAsync(
            request.ScreenId,
            request.StartTime,
            request.EndTime);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "Another show is already scheduled for this screen during the selected time.");
        }

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        await _showRepository.AddAsync(show);

        var createdShow = await _showRepository.GetByIdAsync(show.ShowId);

        if (createdShow == null)
        {
            throw new InvalidOperationException(
                "Unable to retrieve the created show.");
        }

        return MapToResponse(createdShow);
    }

    public async Task<bool> UpdateAsync(
        int showId,
        UpdateShowRequest request)
    {
        var existingShow = await _showRepository.GetByIdAsync(showId);

        if (existingShow == null)
        {
            return false;
        }

        ValidateShowDetails(
            request.StartTime,
            request.EndTime,
            request.TicketPrice);

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
        {
            throw new ArgumentException("Movie not found.");
        }

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
        {
            throw new ArgumentException("Screen not found.");
        }

        bool hasOverlap = await _showRepository.HasOverlappingShowAsync(
            request.ScreenId,
            request.StartTime,
            request.EndTime,
            showId);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "Another show is already scheduled for this screen during the selected time.");
        }

        var updatedShow = new Show
        {
            ShowId = showId,
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        await _showRepository.UpdateAsync(updatedShow);

        return true;
    }

    public async Task<bool> DeleteAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
        {
            return false;
        }

        await _showRepository.DeleteAsync(show);

        return true;
    }

    private static void ValidateShowDetails(
        DateTime startTime,
        DateTime endTime,
        decimal ticketPrice)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentException(
                "End time must be later than start time.");
        }

        if (ticketPrice < 0)
        {
            throw new ArgumentException(
                "Ticket price cannot be negative.");
        }
    }

    private static ShowResponse MapToResponse(Show show)
    {
        return new ShowResponse
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            MovieTitle = show.Movie?.Title ?? string.Empty,
            ScreenId = show.ScreenId,
            ScreenName = show.Screen?.ScreenName ?? string.Empty,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }
}
