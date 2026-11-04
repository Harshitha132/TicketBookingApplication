
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;

    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Seat?> GetByIdAsync(int seatId)
    {
        return await _context.Seats
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SeatId == seatId);
    }

    public async Task<List<Seat>> GetAllByScreenIdAsync(int screenId)
    {
        return await _context.Seats
            .AsNoTracking()
            .Where(s => s.ScreenId == screenId)
            .OrderBy(s => s.RowName)
            .ThenBy(s => s.SeatNumber)
            .ToListAsync();
    }

    public async Task<Seat> AddAsync(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();

        return seat;
    }

    public async Task AddRangeAsync(List<Seat> seats)
    {
        await _context.Seats.AddRangeAsync(seats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seat seat)
    {
        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int seatId)
    {
        return await _context.Seats
            .AnyAsync(s => s.SeatId == seatId);
    }

    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<bool> SeatExistsAsync(
        int screenId,
        string rowName,
        int seatNumber)
    {
        return await _context.Seats.AnyAsync(s =>
            s.ScreenId == screenId &&
            s.RowName == rowName &&
            s.SeatNumber == seatNumber);
    }
}
