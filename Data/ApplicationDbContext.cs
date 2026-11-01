
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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Movie Configuration
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

        // Theatre Configuration
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

        // Screen Configuration
        modelBuilder.Entity<Screen>(entity =>
        {
            entity.ToTable("Screens");

            entity.HasKey(s => s.ScreenId);

            entity.Property(s => s.ScreenId)
                  .ValueGeneratedOnAdd();

            entity.Property(s => s.ScreenName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.TotalSeats)
                  .IsRequired();

            // Theatre - Screen relationship
            entity.HasOne(s => s.Theatre)
                  .WithMany(t => t.Screens)
                  .HasForeignKey(s => s.TheatreId)
                  .OnDelete(DeleteBehavior.Restrict);

            // A theatre cannot have duplicate screen names
            entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                  .IsUnique();
        });

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

            entity.HasOne(s => s.Screen)
                  .WithMany(screen => screen.Seats)
                  .HasForeignKey(s => s.ScreenId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            }).IsUnique();
        });


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

            entity.HasOne(s => s.Movie)
                  .WithMany(m => m.Shows)
                  .HasForeignKey(s => s.MovieId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Screen)
                  .WithMany(screen => screen.Shows)
                  .HasForeignKey(s => s.ScreenId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

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

            entity.HasOne(ss => ss.Show)
                  .WithMany(s => s.ShowSeats)
                  .HasForeignKey(ss => ss.ShowId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ss => ss.Seat)
                  .WithMany()
                  .HasForeignKey(ss => ss.SeatId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                  .IsUnique();
        });


    }
}
