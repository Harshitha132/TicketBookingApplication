
using TicketBookingApplication.DTOs.Shows;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class ShowSeatService : IShowSeatService
{
    private readonly IShowSeatRepository _showSeatRepository;
    private readonly ISeatRepository _seatRepository;

    public ShowSeatService(
        IShowSeatRepository showSeatRepository,
        ISeatRepository seatRepository)
    {
        _showSeatRepository = showSeatRepository;
        _seatRepository = seatRepository;
    }

    // Get all seats for a particular show
    public async Task<List<ShowSeatResponse>> GetByShowIdAsync(int showId)
    {
        if (!await _showSeatRepository.ShowExistsAsync(showId))
        {
            throw new ArgumentException("Show not found.");
        }

        var showSeats =
            await _showSeatRepository.GetByShowIdAsync(showId);

        return showSeats.Select(MapToResponse).ToList();
    }

    // Check whether a seat is available for a show
    public async Task<bool> IsSeatAvailableAsync(int showId, int seatId)
    {
        var showSeat =
            await _showSeatRepository.GetByShowIdAndSeatIdAsync(
                showId, seatId);

        return showSeat != null &&
               showSeat.Status == "Available";
    }

    // Generate ShowSeat records when a show is created
    public async Task GenerateShowSeatsAsync(int showId, int screenId)
    {
        if (!await _showSeatRepository.ShowExistsAsync(showId))
        {
            throw new ArgumentException("Show not found.");
        }

        if (!await _seatRepository.ScreenExistsAsync(screenId))
        {
            throw new ArgumentException("Screen not found.");
        }

        var existingShowSeats =
            await _showSeatRepository.GetByShowIdAsync(showId);

        if (existingShowSeats.Count > 0)
        {
            throw new InvalidOperationException(
                "Show seats have already been generated.");
        }

        var seats =
            await _seatRepository.GetAllByScreenIdAsync(screenId);

        if (seats.Count == 0)
        {
            throw new InvalidOperationException(
                "No seats are configured for this screen.");
        }

        var showSeats = seats.Select(seat => new ShowSeat
        {
            ShowId = showId,
            SeatId = seat.SeatId,
            Status = "Available"
        }).ToList();

        await _showSeatRepository.AddRangeAsync(showSeats);
    }

    // Map entity to response DTO
    private static ShowSeatResponse MapToResponse(ShowSeat showSeat)
    {
        return new ShowSeatResponse
        {
            ShowSeatId = showSeat.ShowSeatId,
            ShowId = showSeat.ShowId,
            SeatId = showSeat.SeatId,
            RowName = showSeat.Seat?.RowName ?? string.Empty,
            SeatNumber = showSeat.Seat?.SeatNumber ?? 0,
            SeatType = showSeat.Seat?.SeatType ?? string.Empty,
            Status = showSeat.Status
        };
    }
}
