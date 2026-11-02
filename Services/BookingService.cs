
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.DTOs.Bookings;
using TicketBookingApplication.Models;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services.Interfaces;

namespace TicketBookingApplication.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IShowRepository _showRepository;
    private readonly IShowSeatRepository _showSeatRepository;
    private readonly ApplicationDbContext _context;

    public BookingService(
        IBookingRepository bookingRepository,
        IShowRepository showRepository,
        IShowSeatRepository showSeatRepository,
        ApplicationDbContext context)
    {
        _bookingRepository = bookingRepository;
        _showRepository = showRepository;
        _showSeatRepository = showSeatRepository;
        _context = context;
    }

    // Create a new booking
    public async Task<BookingResponse> CreateAsync(
        CreateBookingRequest request)
    {
        // Validate customer details
        if (string.IsNullOrWhiteSpace(request.CustomerName) ||
            string.IsNullOrWhiteSpace(request.CustomerEmail) ||
            string.IsNullOrWhiteSpace(request.CustomerPhone))
        {
            throw new ArgumentException(
                "Customer name, email and phone are required.");
        }

        // Validate selected seats
        if (request.SeatIds == null || request.SeatIds.Count == 0)
        {
            throw new ArgumentException(
                "Select at least one seat.");
        }

        if (request.SeatIds.Any(id => id <= 0))
        {
            throw new ArgumentException(
                "Seat IDs must be valid.");
        }

        if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
        {
            throw new ArgumentException(
                "Duplicate seat IDs are not allowed.");
        }

        // Retrieve the selected show
        var show = await _showRepository.GetByIdAsync(request.ShowId);

        if (show == null)
        {
            throw new ArgumentException("Show not found.");
        }

        // Retrieve ShowSeats for the selected show
        var showSeats = await _showSeatRepository.GetByShowIdAsync(
            request.ShowId);

        // Validate that every requested seat exists
        var selectedShowSeats = showSeats
            .Where(ss => request.SeatIds.Contains(ss.SeatId))
            .ToList();

        if (selectedShowSeats.Count != request.SeatIds.Count)
        {
            throw new ArgumentException(
                "One or more selected seats do not belong to this show.");
        }

        // Check seat availability
        if (selectedShowSeats.Any(ss => ss.Status != "Available"))
        {
            throw new InvalidOperationException(
                "One or more selected seats are already booked.");
        }

        // Calculate total using server-side ticket price
        decimal totalAmount =
            show.TicketPrice * selectedShowSeats.Count;

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var booking = new Booking
            {
                BookingReference = GenerateBookingReference(),
                ShowId = request.ShowId,
                CustomerName = request.CustomerName.Trim(),
                CustomerEmail = request.CustomerEmail.Trim(),
                CustomerPhone = request.CustomerPhone.Trim(),
                BookingDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Confirmed"
            };

            // Create BookingSeat records
            foreach (var showSeat in selectedShowSeats)
            {
                booking.BookingSeats.Add(new BookingSeat
                {
                    SeatId = showSeat.SeatId,
                    Price = show.TicketPrice
                });

                // Mark ShowSeat as Booked
                showSeat.Status = "Booked";

                await _showSeatRepository.UpdateAsync(showSeat);
            }

            // Save Booking and associated BookingSeats
            await _bookingRepository.AddAsync(booking);

            await transaction.CommitAsync();

            return MapToResponse(booking);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Retrieve booking by ID
    public async Task<BookingResponse?> GetByIdAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        return booking == null
            ? null
            : MapToResponse(booking);
    }

    // Retrieve all bookings
    public async Task<List<BookingResponse>> GetAllAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();

        return bookings
            .Select(MapToResponse)
            .ToList();
    }

    // Retrieve bookings by Show ID
    public async Task<List<BookingResponse>> GetByShowIdAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
        {
            throw new ArgumentException("Show not found.");
        }

        var bookings = await _bookingRepository.GetByShowIdAsync(showId);

        return bookings
            .Select(MapToResponse)
            .ToList();
    }

    // Cancel an existing booking
    public async Task<BookingResponse?> CancelAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking == null)
        {
            return null;
        }

        if (booking.Status != "Confirmed")
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Restore availability of booked seats
            foreach (var bookingSeat in booking.BookingSeats)
            {
                var showSeat =
                    await _showSeatRepository.GetByShowIdAndSeatIdAsync(
                        booking.ShowId,
                        bookingSeat.SeatId);

                if (showSeat == null)
                {
                    throw new InvalidOperationException(
                        "The corresponding show seat was not found.");
                }

                showSeat.Status = "Available";

                await _showSeatRepository.UpdateAsync(showSeat);
            }

            // Preserve booking history
            booking.Status = "Cancelled";

            await _bookingRepository.UpdateAsync(booking);

            await transaction.CommitAsync();

            return MapToResponse(booking);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Generate a unique booking reference
    private static string GenerateBookingReference()
    {
        return "BK-" + Guid.NewGuid()
            .ToString("N")[..12]
            .ToUpperInvariant();
    }

    // Map Booking entity to BookingResponse DTO
    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            Seats = booking.BookingSeats
                .Select(bs =>
                    $"{bs.Seat?.RowName}{bs.Seat?.SeatNumber}")
                .ToList()
        };
    }
}
