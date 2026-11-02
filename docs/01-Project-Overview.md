# Movie Ticket Booking System

## 1. Project Overview

The **Movie Ticket Booking System** is an ASP.NET Core Web API application designed to manage movie information, theatres, screens, seats, movie shows, and ticket bookings.

The application provides RESTful APIs that allow users or client applications to retrieve movie details, manage theatre information, view available shows, and book movie tickets.

The project follows a layered architecture to separate API handling, business logic, and database operations.

## 2. Project Objectives

- Develop a RESTful Web API using ASP.NET Core.
- Manage movies, theatres, screens, and seats.
- Schedule movie shows in different screens.
- Maintain seat availability for each show.
- Create and manage customer bookings.
- Store booking and seat information in SQL Server.
- Implement a structured architecture using Controllers, Services, Repositories, and Entity Framework Core.

## 3. Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| ASP.NET Core Web API | API development |
| .NET 10 | Development platform |
| Entity Framework Core 10 | ORM and database operations |
| SQL Server | Relational database |
| LINQ | Data querying |
| Scalar / OpenAPI | API documentation support |
| Postman | API testing |
| Git and GitHub | Version control |

## 4. Main Modules

The application contains the following eight main entities:

1. **Movie** – Stores movie details such as title, description, duration, release date, language, and genre.
2. **Theatre** – Stores theatre information, including name and location.
3. **Screen** – Represents individual screens within a theatre.
4. **Seat** – Stores seat information for each screen.
5. **Show** – Maintains movie show schedules, screen details, timings, and ticket prices.
6. **ShowSeat** – Maintains the availability status of seats for individual shows.
7. **Booking** – Stores customer booking information and booking status.
8. **BookingSeat** – Maintains the seats associated with each booking and their prices.

## 5. Application Architecture

The project follows a layered architecture:

**Controller → Service → Repository → ApplicationDbContext → SQL Server**

### Controller Layer
Receives HTTP requests, invokes service methods, and returns appropriate HTTP responses.

### Service Layer
Contains business logic and coordinates operations between controllers and repositories.

### Repository Layer
Handles database operations using Entity Framework Core.

### Data Layer
Contains ApplicationDbContext and entity configurations using EF Core Fluent API.

### Database Layer
SQL Server stores the application data in relational tables.

## 6. Key Functionalities

- Create, retrieve, update, and delete movie information.
- Manage theatres, screens, and seats.
- Create movie shows and generate corresponding show-seat records.
- Retrieve show and seat availability information.
- Book available seats.
- Calculate booking amounts on the server side.
- Cancel confirmed bookings and restore seat availability.
- Maintain booking history.

## 7. Database

**Database Name:** `MovieTicketBookingDB`

The database contains eight primary entity tables:

- Movies
- Theatres
- Screens
- Seats
- Shows
- ShowSeats
- Bookings
- BookingSeats

Entity relationships, validation rules, and database constraints are configured using Entity Framework Core Fluent API.

## 8. API Testing

Postman is used to test the API endpoints.

The Movie module has been tested for:

- Create Movie
- Get All Movies
- Get Movie By ID
- Update Movie
- Delete Movie
- Search Movies

Pagination and title filtering are also supported by the Get All Movies endpoint.

## 9. Project Scope

This is Version 1.0 of the Movie Ticket Booking System.

The initial version focuses on the core movie discovery, show management, seat availability, and booking workflows.

The following features are outside the current Version 1.0 scope:

- User authentication and JWT
- Payment gateway integration
- Temporary seat reservation or seat-hold mechanisms
- Angular frontend
- Advanced concurrency handling

## 10. Conclusion

The Movie Ticket Booking System demonstrates the development of a structured ASP.NET Core Web API application using Entity Framework Core and SQL Server. It applies layered architecture, RESTful API principles, relational database design, and business logic for managing movie ticket bookings.