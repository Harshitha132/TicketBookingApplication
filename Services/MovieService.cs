
using TicketBookingApplication.DTOs.Movies;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    // Get movie by ID
    public async Task<MovieResponse?> GetByIdAsync(int movieId)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return null;

        return MapToResponse(movie);
    }

    // Get all movies
    public async Task<List<MovieResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title = null)
    {
        var movies = await _movieRepository.GetAllAsync(
            pageNumber, pageSize, title);

        return movies.Select(MapToResponse).ToList();
    }

    // Create movie
    public async Task<MovieResponse> CreateAsync(
        CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            Genre = request.Genre
        };

        var createdMovie = await _movieRepository.AddAsync(movie);

        return MapToResponse(createdMovie);
    }

    // Update movie
    public async Task<bool> UpdateAsync(
        int movieId,
        UpdateMovieRequest request)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return false;

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationMinutes = request.DurationMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language;
        movie.Genre = request.Genre;

        await _movieRepository.UpdateAsync(movie);

        return true;
    }

    // Delete movie
    public async Task<bool> DeleteAsync(int movieId)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return false;

        await _movieRepository.DeleteAsync(movie);

        return true;
    }

    // Search movies by title
    public async Task<List<MovieResponse>> SearchAsync(string title)
    {
        var movies = await _movieRepository.GetAllAsync(
            1, int.MaxValue, title);

        return movies.Select(MapToResponse).ToList();
    }

    // Convert Movie entity into MovieResponse DTO
    private static MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}
