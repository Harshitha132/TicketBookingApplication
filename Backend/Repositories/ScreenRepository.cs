
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext _context;

    public ScreenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Screen?> GetByIdAsync(int screenId)
    {
        return await _context.Screens
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ScreenId == screenId);
    }

    public async Task<List<Screen>> GetAllAsync()
    {
        return await _context.Screens
            .AsNoTracking()
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Screens
            .AsNoTracking()
            .Where(s => s.TheatreId == theatreId)
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    public async Task<Screen> AddAsync(Screen screen)
    {
        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();

        return screen;
    }

    public async Task UpdateAsync(Screen screen)
    {
        _context.Screens.Update(screen);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Screen screen)
    {
        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<bool> TheatreExistsAsync(int theatreId)
    {
        return await _context.Theatres
            .AnyAsync(t => t.TheatreId == theatreId);
    }
}
