# Database Design – Movie Ticket Booking System

## 1. Database Overview

The Movie Ticket Booking System uses Microsoft SQL Server as its relational database.

**Database Name:** MovieTicketBookingDB

Entity Framework Core 10 is used to manage database operations, relationships, and schema configuration.

The database consists of eight main tables:

1. Movies
2. Theatres
3. Screens
4. Seats
5. Shows
6. ShowSeats
7. Bookings
8. BookingSeats

## 2. Entity Description

### 2.1 Movies

Stores information about movies available in the system.

| Column | Description |
|---|---|
| MovieId | Primary key |
| Title | Movie title |
| Description | Movie description |
| DurationMinutes | Movie duration |
| ReleaseDate | Movie release date |
| Language | Movie language |
| Genre | Movie genre |

**Constraint:** DurationMinutes must be greater than zero.

### 2.2 Theatres

Stores theatre information.

| Column | Description |
|---|---|
| TheatreId | Primary key |
| TheatreName | Theatre name |
| Location | Theatre location |

### 2.3 Screens

Represents individual screens within a theatre.

| Column | Description |
|---|---|
| ScreenId | Primary key |
| TheatreId | Foreign key referencing Theatres |
| ScreenName | Screen name |

**Constraint:** ScreenName must be unique within a theatre.

### 2.4 Seats

Stores individual seat details for each screen.

| Column | Description |
|---|---|
| SeatId | Primary key |
| ScreenId | Foreign key referencing Screens |
| RowName | Seat row identifier |
| SeatNumber | Seat number |
| SeatType | Type of seat |

**Constraints:**
- RowName has a maximum length of 5 characters.
- SeatNumber must be greater than zero.
- The combination of ScreenId, RowName, and SeatNumber must be unique.

### 2.5 Shows

Stores movie show schedules.

| Column | Description |
|---|---|
| ShowId | Primary key |
| MovieId | Foreign key referencing Movies |
| ScreenId | Foreign key referencing Screens |
| StartTime | Show start date and time |
| EndTime | Show end date and time |
| TicketPrice | Ticket price |

**Constraints:**
- EndTime must be greater than StartTime.
- TicketPrice must be greater than or equal to zero.

### 2.6 ShowSeats

Maintains the availability of individual seats for each show.

| Column | Description |
|---|---|
| ShowSeatId | Primary key |
| ShowId | Foreign key referencing Shows |
| SeatId | Foreign key referencing Seats |
| Status | Seat availability status |

**Constraints:**
- The combination of ShowId and SeatId must be unique.
- Status must be either Available or Booked.

### 2.7 Bookings

Stores customer booking information.

| Column | Description |
|---|---|
| BookingId | Primary key |
| BookingReference | Unique booking reference |
| ShowId | Foreign key referencing Shows |
| CustomerName | Customer name |
| CustomerEmail | Customer email |
| CustomerPhone | Customer phone number |
| BookingDate | Booking date |
| TotalAmount | Total booking amount |
| Status | Booking status |

**Constraints:**
- BookingReference must be unique.
- Status must be Confirmed or Cancelled.

### 2.8 BookingSeats

Stores the seats associated with individual bookings.

| Column | Description |
|---|---|
| BookingSeatId | Primary key |
| BookingId | Foreign key referencing Bookings |
| SeatId | Foreign key referencing Seats |
| Price | Price of the booked seat |

**Constraint:** Price must be greater than or equal to zero.

## 3. Entity Relationships

The database follows these relationships:

| Parent Entity | Child Entity | Relationship |
|---|---|---|
| Theatre | Screen | One-to-Many |
| Screen | Seat | One-to-Many |
| Movie | Show | One-to-Many |
| Screen | Show | One-to-Many |
| Show | ShowSeat | One-to-Many |
| Seat | ShowSeat | One-to-Many |
| Show | Booking | One-to-Many |
| Booking | BookingSeat | One-to-Many |

## 4. Relationship Explanation

- One theatre can contain multiple screens.
- One screen can contain multiple seats.
- One movie can have multiple scheduled shows.
- One screen can host multiple shows at different times.
- One show has multiple ShowSeat records.
- One seat can appear in multiple ShowSeat records across different shows.
- One show can have multiple bookings.
- One booking can contain multiple booked seats.

## 5. Database Constraints and Validation

The database uses constraints to maintain data integrity.

Important constraints include:

- Primary keys identify individual records.
- Foreign keys maintain relationships between tables.
- Unique constraints prevent duplicate screen names within a theatre, duplicate seats within a screen, duplicate show-seat combinations, and duplicate booking references.
- Check constraints validate durations, prices, show timings, and status values.
- Restrictive delete behavior protects dependent booking and history records.

## 6. Booking and Seat Availability Design

The ShowSeats table is used to track seat availability separately for every show.

When a show is created, ShowSeat records are generated for the seats belonging to its screen.

During booking:
1. The requested ShowSeat records are validated.
2. The system checks whether the seats are Available.
3. The booking amount is calculated on the server.
4. A Booking record is created.
5. BookingSeat records are created.
6. The corresponding ShowSeat records are updated to Booked.

These operations are performed within a database transaction.

During cancellation:
1. The booking status is changed to Cancelled.
2. The associated ShowSeat records are restored to Available.
3. The booking and booking-seat history is preserved.

Cancellation is permitted only for Confirmed bookings.

## 7. Entity Framework Core Configuration

The database schema is configured using `ApplicationDbContext` and EF Core Fluent API.

The configuration includes:

- Entity-to-table mapping.
- Primary and foreign keys.
- Property lengths and data types.
- Unique indexes.
- Check constraints.
- Relationship configurations.
- Delete behaviors.

## 8. Initial Database Migration

The initial migration was created after configuring the entities and relationships.

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

The migration creates the database schema based on the configured entity model.

## 9. Conclusion

The database design uses eight related entities to manage movies, theatres, screens, seats, shows, and bookings. Primary keys, foreign keys, unique constraints, check constraints, and transactions help maintain data consistency and support the core booking workflow.