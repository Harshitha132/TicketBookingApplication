# Data Layer

## 1. Introduction

The Data Layer manages database connectivity and entity configurations in the Movie Ticket Booking System.

The application uses Entity Framework Core 10 with SQL Server as its database provider.

The main component of this layer is `ApplicationDbContext`.

## 2. ApplicationDbContext

`ApplicationDbContext` is responsible for managing entity objects and communicating with the SQL Server database.

It inherits from `DbContext` and exposes DbSet properties for the eight entities.

### DbSet Properties

```csharp
public DbSet<Movie> Movies { get; set; }
public DbSet<Theatre> Theatres { get; set; }
public DbSet<Screen> Screens { get; set; }
public DbSet<Seat> Seats { get; set; }
public DbSet<Show> Shows { get; set; }
public DbSet<ShowSeat> ShowSeats { get; set; }
public DbSet<Booking> Bookings { get; set; }
public DbSet<BookingSeat> BookingSeats { get; set; }
```

Each DbSet represents a database table.

## 3. Fluent API Configuration

Entity Framework Core Fluent API is used to configure the database schema.

The configuration is applied through the `OnModelCreating()` method in `ApplicationDbContext`.

The configurations include:

- Table and column mapping.
- Primary key configuration.
- Foreign key relationships.
- Property maximum lengths.
- Required properties.
- Unique indexes.
- Check constraints.
- Delete behaviors.

## 4. Entity Relationships

The Data Layer configures the following relationships:

- Theatre and Screen.
- Screen and Seat.
- Movie and Show.
- Screen and Show.
- Show and ShowSeat.
- Seat and ShowSeat.
- Show and Booking.
- Booking and BookingSeat.
- Seat and BookingSeat.

These relationships ensure referential integrity between related database records.

## 5. Database Connection

The database connection string is maintained in `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

The DbContext is registered in `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

## 6. EF Core Migrations

Migrations are used to maintain and update the database schema based on changes to the entity models and configurations.

### Initial Migration

```powershell
dotnet ef migrations add InitialCreate
```

### Update Database

```powershell
dotnet ef database update
```

The generated migration files are stored in the `Migrations` folder.

## 7. Database Transaction Support

Transactions are important for operations that modify multiple related records.

The booking and cancellation workflows require transactional operations to maintain consistency between:

- Bookings
- BookingSeats
- ShowSeats

A transaction ensures that related changes are committed together or rolled back if an operation fails.

## 8. Benefits of the Data Layer

- Centralizes database configuration.
- Maintains entity relationships.
- Supports database migrations.
- Improves data integrity.
- Separates database configuration from business logic.
- Supports transactional operations.

## 9. Conclusion

The Data Layer provides database connectivity and entity configuration using Entity Framework Core. `ApplicationDbContext`, Fluent API, SQL Server, and migrations form the foundation for database operations in the application.