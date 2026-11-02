# Controller Layer

## 1. Introduction

The Controller Layer is the entry point of the Movie Ticket Booking System Web API.

It receives HTTP requests from clients, invokes the appropriate service methods, and returns HTTP responses.

Controllers are implemented using ASP.NET Core Web API and follow RESTful API principles.

## 2. Controller Architecture

The request flow is:

**Client → Controller → Service → Repository → Database**

Controllers communicate with the Service Layer through dependency injection.

They are responsible for handling HTTP methods, route parameters, query parameters, request bodies, and response status codes.

## 3. Controllers

The application contains the following controllers:

| Controller | Responsibility |
|---|---|
| MoviesController | Movie management |
| TheatresController | Theatre management |
| ScreensController | Screen management |
| SeatsController | Seat management |
| ShowsController | Show scheduling and show-seat operations |
| BookingsController | Booking and cancellation operations |

ShowSeat operations are managed through `ShowsController`; a separate ShowSeatsController is not part of the design.

## 4. MoviesController

The MoviesController handles movie-related API operations.

Base route:

```http
/api/movies
```

| HTTP Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/movies` | Create a movie |
| GET | `/api/movies` | Retrieve movies with pagination and optional title filtering |
| GET | `/api/movies/{id}` | Retrieve a movie by ID |
| PUT | `/api/movies/{id}` | Update a movie |
| DELETE | `/api/movies/{id}` | Delete a movie |
| GET | `/api/movies/search?title=Vikram` | Search movies by title |

### Pagination and Filtering

The Get All Movies endpoint supports:

- `pageNumber` – Page number to retrieve.
- `pageSize` – Number of records per page.
- `title` – Optional title filter.

Example:

```http
GET /api/movies?pageNumber=1&pageSize=10&title=Vikram
```

## 5. Other Controller Responsibilities

### TheatresController
Manages theatre information and theatre retrieval operations.

### ScreensController
Manages screens associated with theatres.

### SeatsController
Manages seat information associated with screens.

### ShowsController
Manages show scheduling and operations related to show-seat availability.

### BookingsController
Handles customer booking creation, booking retrieval, and cancellation operations.

## 6. Dependency Injection

Controllers receive service dependencies through constructor injection.

Example:

```csharp
private readonly IMovieService _movieService;

public MoviesController(IMovieService movieService)
{
    _movieService = movieService;
}
```

This keeps controllers independent of concrete service implementations.

## 7. HTTP Status Codes

The API uses appropriate HTTP status codes to indicate the result of requests.

| Status Code | Meaning |
|---|---|
| 200 OK | Request completed successfully |
| 201 Created | Resource created successfully |
| 204 No Content | Request succeeded without a response body |
| 400 Bad Request | Invalid request data |
| 404 Not Found | Requested resource does not exist |
| 405 Method Not Allowed | HTTP method is not supported for the endpoint |
| 500 Internal Server Error | Unexpected server-side error |

## 8. API Testing

Postman is used to verify controller endpoints by sending HTTP requests and examining responses.

The MoviesController endpoints have been tested for basic CRUD operations and movie search.

## 9. Conclusion

The Controller Layer exposes the application's functionality through RESTful API endpoints. It handles HTTP communication and delegates business operations to the Service Layer, maintaining separation of responsibilities throughout the application.