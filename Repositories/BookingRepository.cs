
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get booking details by ID
    public async Task<Booking?> GetByIdAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Show)
                .ThenInclude(s => s!.Movie)
            .Include(b => b.Show)
                .ThenInclude(s => s!.Screen)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    // Get all bookings
    public async Task<List<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Show)
                .ThenInclude(s => s!.Movie)
            .Include(b => b.Show)
                .ThenInclude(s => s!.Screen)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    // Get bookings by Show ID
    public async Task<List<Booking>> GetByShowIdAsync(int showId)
    {
        return await _context.Bookings
            .Include(b => b.Show)
                .ThenInclude(s => s!.Movie)
            .Include(b => b.Show)
                .ThenInclude(s => s!.Screen)
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .Where(b => b.ShowId == showId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    // Add a new booking
    public async Task<Booking> AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);

        await _context.SaveChangesAsync();

        return booking;
    }

    // Update an existing booking
    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);

        await _context.SaveChangesAsync();
    }
}
