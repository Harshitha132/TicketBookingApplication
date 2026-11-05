
using TicketBookingApplication.DTOs.Seats;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;

    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<SeatResponse?> GetByIdAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        return seat == null ? null : MapToResponse(seat);
    }

    public async Task<List<SeatResponse>> GetAllByScreenIdAsync(int screenId)
    {
        if (!await _seatRepository.ScreenExistsAsync(screenId))
            throw new ArgumentException("Screen not found.");

        var seats = await _seatRepository.GetAllByScreenIdAsync(screenId);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse> CreateAsync(CreateSeatRequest request)
    {
        if (!await _seatRepository.ScreenExistsAsync(request.ScreenId))
            throw new ArgumentException("Screen not found.");

        if (await _seatRepository.SeatExistsAsync(
            request.ScreenId, request.RowName, request.SeatNumber))
        {
            throw new InvalidOperationException("This seat already exists.");
        }

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType
        };

        var createdSeat = await _seatRepository.AddAsync(seat);

        return MapToResponse(createdSeat);
    }

    public async Task<List<SeatResponse>> BulkCreateAsync(
        BulkCreateSeatsRequest request)
    {
        if (!await _seatRepository.ScreenExistsAsync(request.ScreenId))
            throw new ArgumentException("Screen not found.");

        var duplicateInRequest = request.Seats
            .GroupBy(s => new { s.RowName, s.SeatNumber })
            .Any(g => g.Count() > 1);

        if (duplicateInRequest)
            throw new InvalidOperationException(
                "Duplicate seat positions found in the request.");

        var existingSeats = await _seatRepository
            .GetAllByScreenIdAsync(request.ScreenId);

        var existingPositions = existingSeats
            .Select(s => $"{s.RowName}:{s.SeatNumber}")
            .ToHashSet();

        foreach (var item in request.Seats)
        {
            if (existingPositions.Contains($"{item.RowName}:{item.SeatNumber}"))
            {
                throw new InvalidOperationException(
                    $"Seat {item.RowName}{item.SeatNumber} already exists.");
            }
        }

        var seats = request.Seats.Select(item => new Seat
        {
            ScreenId = request.ScreenId,
            RowName = item.RowName,
            SeatNumber = item.SeatNumber,
            SeatType = item.SeatType
        }).ToList();

        await _seatRepository.AddRangeAsync(seats);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<bool> UpdateAsync(
        int seatId,
        UpdateSeatRequest request)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return false;

        var screenSeats = await _seatRepository
            .GetAllByScreenIdAsync(seat.ScreenId);

        var duplicate = screenSeats.Any(s =>
            s.SeatId != seatId &&
            s.RowName == request.RowName &&
            s.SeatNumber == request.SeatNumber);

        if (duplicate)
            throw new InvalidOperationException(
                "Another seat already uses this position.");

        seat.RowName = request.RowName;
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType;

        await _seatRepository.UpdateAsync(seat);

        return true;
    }

    public async Task<bool> DeleteAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return false;

        await _seatRepository.DeleteAsync(seat);

        return true;
    }

    private static SeatResponse MapToResponse(Seat seat)
    {
        return new SeatResponse
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}
