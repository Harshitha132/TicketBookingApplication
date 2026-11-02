# Service Layer

## 1. Introduction

The Service Layer contains the business logic of the Movie Ticket Booking System.

It acts as an intermediate layer between Controllers and Repositories. Controllers use services to perform application operations, while services communicate with repositories to access data.

## 2. Service Architecture

**Controller → Service → Repository**

The Service Layer is responsible for processing requests, applying business rules, coordinating operations, and returning results to controllers.

## 3. Service Interfaces

The application defines service interfaces for its functional modules.

| Interface | Responsibility |
|---|---|
| IMovieService | Movie-related business operations |
| ITheatreService | Theatre-related business operations |
| IScreenService | Screen-related business operations |
| ISeatService | Seat-related business operations |
| IShowService | Show-related business operations |
| IShowSeatService | Show-seat-related business operations |
| IBookingService | Booking-related business operations |

## 4. Service Implementations

Each service interface has a corresponding implementation.

| Service | Responsibility |
|---|---|
| MovieService | Processes movie operations |
| TheatreService | Processes theatre operations |
| ScreenService | Processes screen operations |
| SeatService | Processes seat operations |
| ShowService | Processes show operations |
| ShowSeatService | Processes show-seat operations |
| BookingService | Processes booking operations |

## 5. Business Logic Responsibilities

### 5.1 Movie Service

- Creates movie records.
- Retrieves movie information.
- Updates movie details.
- Deletes movies.
- Searches movies by title.
- Supports pagination and title filtering through the movie retrieval operation.

### 5.2 Theatre, Screen, and Seat Services

- Manage theatre information.
- Manage screens associated with theatres.
- Manage seats associated with screens.
- Apply relevant validation rules.

### 5.3 Show Service

- Manages movie show information.
- Validates show details.
- Coordinates the generation of ShowSeat records when a show is created.

### 5.4 ShowSeat Service

- Manages seat availability information for individual shows.
- Supports retrieving and updating show-seat status as required by the booking workflow.

### 5.5 Booking Service

- Validates booking requests.
- Checks seat availability.
- Calculates booking amounts on the server side.
- Creates booking and booking-seat records.
- Updates seat availability.
- Handles booking cancellation.

## 6. Transaction Management

Booking and cancellation operations involve multiple related database changes.

These operations use database transactions to maintain data consistency.

### Booking Transaction

1. Validate the requested seats.
2. Check whether the seats are available.
3. Calculate the total booking amount.
4. Create the booking record.
5. Create the associated booking-seat records.
6. Update the corresponding ShowSeat records to Booked.
7. Commit the transaction.

If an operation fails, the transaction is rolled back.

### Cancellation Transaction

1. Verify that the booking exists.
2. Verify that its status is Confirmed.
3. Update the booking status to Cancelled.
4. Restore the associated ShowSeat records to Available.
5. Commit the transaction.

The booking history is preserved.

## 7. Dependency Injection

Service implementations are registered in `Program.cs` using scoped dependency injection.

```csharp
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IShowSeatService, ShowSeatService>();
builder.Services.AddScoped<IBookingService, BookingService>();
```

This allows controllers to receive service dependencies through constructor injection.

## 8. Benefits of the Service Layer

- Separates business logic from API handling.
- Keeps controllers focused on HTTP requests and responses.
- Centralizes business rules.
- Improves maintainability and testability.
- Coordinates operations involving multiple repositories.

## 9. Conclusion

The Service Layer manages the business logic of the Movie Ticket Booking System. It coordinates repository operations and applies the rules required for movie management, show scheduling, seat availability, and ticket booking.