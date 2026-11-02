# Models and DTOs

## 1. Introduction

The Movie Ticket Booking System uses Models and Data Transfer Objects (DTOs) to represent application data and transfer information between API clients and the backend.

Models represent the database entities, while DTOs define the data accepted from and returned to API clients.

## 2. Models

The application contains eight entity models.

| Model | Purpose |
|---|---|
| Movie | Represents movie information |
| Theatre | Represents theatre details |
| Screen | Represents screens within theatres |
| Seat | Represents seats within screens |
| Show | Represents scheduled movie shows |
| ShowSeat | Represents seat availability for a show |
| Booking | Represents customer bookings |
| BookingSeat | Represents seats associated with a booking |

### 2.1 Movie Model

Stores movie details such as title, description, duration, release date, language, and genre.

### 2.2 Theatre Model

Stores the theatre name and location and maintains its relationship with screens.

### 2.3 Screen Model

Represents a screen belonging to a theatre and maintains its relationship with seats and shows.

### 2.4 Seat Model

Represents an individual seat within a screen, including its row, number, and type.

### 2.5 Show Model

Represents a scheduled movie show with its movie, screen, start time, end time, and ticket price.

### 2.6 ShowSeat Model

Maintains the availability status of each seat for a particular show.

### 2.7 Booking Model

Stores customer details, booking reference, show information, booking date, total amount, and booking status.

### 2.8 BookingSeat Model

Stores the seats included in a booking and the price associated with each seat.

## 3. Data Transfer Objects (DTOs)

DTOs are used to control the data exchanged between the client and the API.

They help separate API request and response structures from the database entity models.

The project organizes DTOs into module-specific folders:

- DTOs/Movies
- DTOs/Theatres
- DTOs/Screens
- DTOs/Seats
- DTOs/Shows
- DTOs/Bookings

## 4. Movie DTOs

The Movie module uses request DTOs to receive movie creation and update information.

- `CreateMovieRequest` – Contains the information required to create a movie.
- `UpdateMovieRequest` – Contains the information required to update an existing movie.

The API returns movie information through the response objects defined by the implementation.

## 5. DTO Design Principles

- Keep request and response data separate from database entities.
- Avoid exposing unnecessary database properties.
- Use appropriate data types for request properties.
- Apply validation to incoming request data.
- Organize DTOs by functional module.
- Keep API contracts clear and consistent.

## 6. Model Relationships

The models contain navigation properties to represent entity relationships.

For example:

- Theatre contains a collection of Screens.
- Screen contains a collection of Seats.
- Movie contains a collection of Shows.
- Show contains collections of ShowSeats and Bookings.
- Booking contains a collection of BookingSeats.

These relationships are configured through Entity Framework Core.

## 7. Conclusion

Models represent the database structure, while DTOs define the data exchanged through API endpoints. Separating these responsibilities improves maintainability, data validation, and the organization of the application.