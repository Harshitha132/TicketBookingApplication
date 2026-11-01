
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
    }
}
