
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;

namespace TicketBookingApplication.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get movie by ID
    public async Task<Movie?> GetByIdAsync(int movieId)
    {
        return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MovieId == movieId);
    }

    // Get all movies with pagination
    public async Task<List<Movie>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title = null)
    {
        var query = _context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(m =>
                m.Title.Contains(title));
        }

        return await query
            .OrderBy(m => m.MovieId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Add movie
    public async Task<Movie> AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        return movie;
    }

    // Update movie
    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    // Delete movie
    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }

    // Check whether movie exists
    public async Task<bool> ExistsAsync(int movieId)
    {
        return await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);
    }
}
