
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class ShowSeatRepository : IShowSeatRepository
{
    private readonly ApplicationDbContext _context;

    public ShowSeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShowSeat?> GetByIdAsync(int showSeatId)
    {
        return await _context.ShowSeats
            .AsNoTracking()
            .Include(ss => ss.Seat)
            .FirstOrDefaultAsync(ss => ss.ShowSeatId == showSeatId);
    }

    public async Task<List<ShowSeat>> GetByShowIdAsync(int showId)
    {
        return await _context.ShowSeats
            .AsNoTracking()
            .Include(ss => ss.Seat)
            .Where(ss => ss.ShowId == showId)
            .OrderBy(ss => ss.Seat!.RowName)
            .ThenBy(ss => ss.Seat!.SeatNumber)
            .ToListAsync();
    }

    public async Task<ShowSeat?> GetByShowIdAndSeatIdAsync(
        int showId,
        int seatId)
    {
        return await _context.ShowSeats
            .FirstOrDefaultAsync(ss =>
                ss.ShowId == showId &&
                ss.SeatId == seatId);
    }

    public async Task AddRangeAsync(List<ShowSeat> showSeats)
    {
        await _context.ShowSeats.AddRangeAsync(showSeats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ShowSeat showSeat)
    {
        _context.ShowSeats.Update(showSeat);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ShowExistsAsync(int showId)
    {
        return await _context.Shows
            .AnyAsync(s => s.ShowId == showId);
    }

    public async Task<bool> SeatExistsAsync(int seatId)
    {
        return await _context.Seats
            .AnyAsync(s => s.SeatId == seatId);
    }
}
