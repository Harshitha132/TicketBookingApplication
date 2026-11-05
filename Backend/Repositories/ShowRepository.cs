
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext _context;

    public ShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get show by ID
    public async Task<Show?> GetByIdAsync(int showId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    // Get all shows with optional filters
    public async Task<List<Show>> GetAllAsync(
        DateTime? date = null,
        int? movieId = null,
        int? theatreId = null)
    {
        var query = _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .AsQueryable();

        if (date.HasValue)
        {
            var startOfDay = date.Value.Date;
            var endOfDay = startOfDay.AddDays(1);

            query = query.Where(s =>
                s.StartTime >= startOfDay &&
                s.StartTime < endOfDay);
        }

        if (movieId.HasValue)
        {
            query = query.Where(s => s.MovieId == movieId.Value);
        }

        if (theatreId.HasValue)
        {
            query = query.Where(s =>
                s.Screen != null &&
                s.Screen.TheatreId == theatreId.Value);
        }

        return await query
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Get shows by movie ID
    public async Task<List<Show>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Get shows by theatre ID
    public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
            .Where(s => s.Screen != null &&
                        s.Screen.TheatreId == theatreId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Add show
    public async Task<Show> AddAsync(Show show)
    {
        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();

        return show;
    }

    // Update show
    public async Task UpdateAsync(Show show)
    {
        _context.Shows.Update(show);
        await _context.SaveChangesAsync();
    }

    // Delete show
    public async Task DeleteAsync(Show show)
    {
        _context.Shows.Remove(show);
        await _context.SaveChangesAsync();
    }

    // Check movie existence
    public async Task<bool> MovieExistsAsync(int movieId)
    {
        return await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);
    }

    // Check screen existence
    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    // Check overlapping shows on the same screen
    public async Task<bool> HasOverlappingShowAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null)
    {
        var query = _context.Shows
            .Where(s =>
                s.ScreenId == screenId &&
                s.StartTime < endTime &&
                s.EndTime > startTime);

        if (excludeShowId.HasValue)
        {
            query = query.Where(s =>
                s.ShowId != excludeShowId.Value);
        }

        return await query.AnyAsync();
    }
}
