# Repository Layer

## 1. Introduction

The Repository Layer is responsible for handling database operations in the Movie Ticket Booking System.

It separates data access logic from business logic, making the application easier to maintain, test, and extend.

The application follows the Repository Pattern using interfaces and concrete repository classes.

## 2. Repository Architecture

The data access flow is:

**Controller → Service → Repository → ApplicationDbContext → SQL Server**

The Service Layer communicates with repository interfaces rather than directly accessing the database.

## 3. Repository Interfaces

Repository interfaces define the operations that can be performed on each entity.

The project contains the following repository interfaces:

| Interface | Responsibility |
|---|---|
| IMovieRepository | Movie data operations |
| ITheatreRepository | Theatre data operations |
| IScreenRepository | Screen data operations |
| ISeatRepository | Seat data operations |
| IShowRepository | Show data operations |
| IShowSeatRepository | Show-seat data operations |
| IBookingRepository | Booking data operations |

These interfaces provide contracts for the corresponding repository implementations.

## 4. Repository Implementations

The concrete repository classes implement the repository interfaces.

| Repository | Responsibility |
|---|---|
| MovieRepository | Handles movie database operations |
| TheatreRepository | Handles theatre database operations |
| ScreenRepository | Handles screen database operations |
| SeatRepository | Handles seat database operations |
| ShowRepository | Handles show database operations |
| ShowSeatRepository | Handles show-seat database operations |
| BookingRepository | Handles booking database operations |

## 5. Database Access

Repositories use `ApplicationDbContext` to interact with SQL Server through Entity Framework Core.

Common database operations include:

- Retrieving records.
- Retrieving records by ID.
- Adding new records.
- Updating existing records.
- Deleting records.
- Querying related entities.

Asynchronous Entity Framework Core methods are used where appropriate.

Examples include:

- `ToListAsync()`
- `FirstOrDefaultAsync()`
- `AddAsync()`
- `SaveChangesAsync()`

## 6. Dependency Injection

Repository implementations are registered in `Program.cs` using scoped dependency injection.

```csharp
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowSeatRepository, ShowSeatRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
```

Scoped lifetime creates a repository instance for each HTTP request scope.

## 7. Benefits of the Repository Pattern

- Separates database operations from business logic.
- Improves code organization.
- Reduces direct database dependencies in services.
- Supports easier testing and maintenance.
- Encourages loose coupling through interfaces.

## 8. Conclusion

The Repository Layer provides a structured approach to database access. By using repository interfaces and implementations, the application keeps data access logic separate from the business logic and API handling layers.