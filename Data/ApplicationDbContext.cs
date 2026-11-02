
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Models;

namespace TicketBookingApplication.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Movie> Movies { get; set; }

    public DbSet<Theatre> Theatres { get; set; }

    public DbSet<Screen> Screens { get; set; }

    public DbSet<Seat> Seats { get; set; }

    public DbSet<Show> Shows { get; set; }

    public DbSet<ShowSeat> ShowSeats { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<BookingSeat> BookingSeats { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================
        // 1. MOVIE CONFIGURATION
        // =========================================

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies", table =>
            {
                table.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] > 0");
            });

            entity.HasKey(m => m.MovieId);

            entity.Property(m => m.MovieId)
                  .ValueGeneratedOnAdd();

            entity.Property(m => m.Title)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(m => m.Description)
                  .HasMaxLength(500);

            entity.Property(m => m.DurationMinutes)
                  .IsRequired();

            entity.Property(m => m.ReleaseDate)
                  .HasColumnType("date")
                  .IsRequired();

            entity.Property(m => m.Language)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(m => m.Genre)
                  .IsRequired()
                  .HasMaxLength(50);
        });


        // =========================================
        // 2. THEATRE CONFIGURATION
        // =========================================

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.ToTable("Theatres");

            entity.HasKey(t => t.TheatreId);

            entity.Property(t => t.TheatreId)
                  .ValueGeneratedOnAdd();

            entity.Property(t => t.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(t => t.Location)
                  .IsRequired()
                  .HasMaxLength(200);
        });


        // =========================================
        // 3. SCREEN CONFIGURATION
        // =========================================

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.ToTable("Screens");

            entity.HasKey(s => s.ScreenId);

            entity.Property(s => s.ScreenId)
                  .ValueGeneratedOnAdd();

            entity.Property(s => s.ScreenName)
                  .IsRequired()
                  .HasMaxLength(50);

            // Retained from your existing implementation
            entity.Property(s => s.TotalSeats)
                  .IsRequired();

            // Theatre - Screen relationship
            entity.HasOne(s => s.Theatre)
                  .WithMany(t => t.Screens)
                  .HasForeignKey(s => s.TheatreId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Unique screen name within a theatre
            entity.HasIndex(s => new
            {
                s.TheatreId,
                s.ScreenName
            }).IsUnique();
        });


        // =========================================
        // 4. SEAT CONFIGURATION
        // =========================================

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seats", table =>
            {
                table.HasCheckConstraint(
                    "CK_Seats_SeatNumber",
                    "[SeatNumber] > 0");
            });

            entity.HasKey(s => s.SeatId);

            entity.Property(s => s.SeatId)
                  .ValueGeneratedOnAdd();

            entity.Property(s => s.RowName)
                  .IsRequired()
                  .HasMaxLength(5);

            entity.Property(s => s.SeatNumber)
                  .IsRequired();

            entity.Property(s => s.SeatType)
                  .IsRequired()
                  .HasMaxLength(30);

            // Screen - Seat relationship
            entity.HasOne(s => s.Screen)
                  .WithMany(screen => screen.Seats)
                  .HasForeignKey(s => s.ScreenId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate seat positions within a screen
            entity.HasIndex(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            }).IsUnique();
        });


        // =========================================
        // 5. SHOW CONFIGURATION
        // =========================================

        modelBuilder.Entity<Show>(entity =>
        {
            entity.ToTable("Shows", table =>
            {
                table.HasCheckConstraint(
                    "CK_Shows_EndTime",
                    "[EndTime] > [StartTime]");

                table.HasCheckConstraint(
                    "CK_Shows_TicketPrice",
                    "[TicketPrice] >= 0");
            });

            entity.HasKey(s => s.ShowId);

            entity.Property(s => s.ShowId)
                  .ValueGeneratedOnAdd();

            entity.Property(s => s.StartTime)
                  .IsRequired();

            entity.Property(s => s.EndTime)
                  .IsRequired();

            entity.Property(s => s.TicketPrice)
                  .HasPrecision(10, 2)
                  .IsRequired();

            // Movie - Show relationship
            entity.HasOne(s => s.Movie)
                  .WithMany(m => m.Shows)
                  .HasForeignKey(s => s.MovieId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Screen - Show relationship
            entity.HasOne(s => s.Screen)
                  .WithMany(screen => screen.Shows)
                  .HasForeignKey(s => s.ScreenId)
                  .OnDelete(DeleteBehavior.Restrict);
        });


        // =========================================
        // 6. SHOWSEAT CONFIGURATION
        // =========================================

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.ToTable("ShowSeats", table =>
            {
                table.HasCheckConstraint(
                    "CK_ShowSeats_Status",
                    "[Status] IN ('Available', 'Booked')");
            });

            entity.HasKey(ss => ss.ShowSeatId);

            entity.Property(ss => ss.ShowSeatId)
                  .ValueGeneratedOnAdd();

            entity.Property(ss => ss.Status)
                  .IsRequired()
                  .HasMaxLength(20);

            // Show - ShowSeat relationship
            entity.HasOne(ss => ss.Show)
                  .WithMany(s => s.ShowSeats)
                  .HasForeignKey(ss => ss.ShowId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Seat - ShowSeat relationship
            entity.HasOne(ss => ss.Seat)
                  .WithMany()
                  .HasForeignKey(ss => ss.SeatId)
                  .OnDelete(DeleteBehavior.Restrict);

            // A seat can appear only once per show
            entity.HasIndex(ss => new
            {
                ss.ShowId,
                ss.SeatId
            }).IsUnique();
        });


        // =========================================
        // 7. BOOKING CONFIGURATION
        // =========================================

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings", table =>
            {
                table.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] >= 0");

                table.HasCheckConstraint(
                    "CK_Bookings_Status",
                    "[Status] IN ('Confirmed', 'Cancelled')");
            });

            entity.HasKey(b => b.BookingId);

            entity.Property(b => b.BookingId)
                  .ValueGeneratedOnAdd();

            entity.Property(b => b.BookingReference)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.Property(b => b.CustomerName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(b => b.CustomerEmail)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(b => b.CustomerPhone)
                  .IsRequired()
                  .HasMaxLength(15);

            entity.Property(b => b.BookingDate)
                  .IsRequired();

            entity.Property(b => b.TotalAmount)
                  .HasPrecision(10, 2)
                  .IsRequired();

            entity.Property(b => b.Status)
                  .IsRequired()
                  .HasMaxLength(20);

            // Show - Booking relationship
            entity.HasOne(b => b.Show)
                  .WithMany()
                  .HasForeignKey(b => b.ShowId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Booking - BookingSeat relationship
            entity.HasMany(b => b.BookingSeats)
                  .WithOne(bs => bs.Booking)
                  .HasForeignKey(bs => bs.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Unique booking reference
            entity.HasIndex(b => b.BookingReference)
                  .IsUnique();
        });


        // =========================================
        // 8. BOOKINGSEAT CONFIGURATION
        // =========================================

        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.ToTable("BookingSeats", table =>
            {
                table.HasCheckConstraint(
                    "CK_BookingSeats_Price",
                    "[Price] >= 0");
            });

            entity.HasKey(bs => bs.BookingSeatId);

            entity.Property(bs => bs.BookingSeatId)
                  .ValueGeneratedOnAdd();

            entity.Property(bs => bs.Price)
                  .HasPrecision(10, 2)
                  .IsRequired();

            // Seat - BookingSeat relationship
            entity.HasOne(bs => bs.Seat)
                  .WithMany()
                  .HasForeignKey(bs => bs.SeatId)
                  .OnDelete(DeleteBehavior.Restrict);
        });


        base.OnModelCreating(modelBuilder);
    }
}
