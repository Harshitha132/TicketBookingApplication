
using TicketBookingApplication.DTOs.Seats;

namespace TicketBookingApplication.Services.Interfaces;

public interface ISeatService
{
    Task<SeatResponse?> GetByIdAsync(int seatId);

    Task<List<SeatResponse>> GetAllByScreenIdAsync(int screenId);

    Task<SeatResponse> CreateAsync(CreateSeatRequest request);

    Task<List<SeatResponse>> BulkCreateAsync(
        BulkCreateSeatsRequest request);

    Task<bool> UpdateAsync(
        int seatId,
        UpdateSeatRequest request);

    Task<bool> DeleteAsync(int seatId);
}
