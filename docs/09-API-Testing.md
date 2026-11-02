# API Testing – Postman

## 1. Introduction

Postman is used to test the RESTful APIs developed for the Movie Ticket Booking System.

It helps verify HTTP methods, request payloads, response data, status codes, and API behavior.

## 2. Postman Collection

A Postman collection named **Movie Ticket Booking System** was created.

The collection contains separate folders for testing individual modules.

The Movies folder contains requests for movie management and search operations.

## 3. Base URL

The HTTP base URL used during local development is:

```text
http://localhost:5246/api
```

The Movies API base URL is:

```text
http://localhost:5246/api/movies
```

## 4. Movies API Testing

### 4.1 Create Movie

**Method:** POST

**Endpoint:**

```http
POST http://localhost:5246/api/movies
```

**Request Body:**

```json
{
  "title": "Vikram",
  "description": "An action thriller movie",
  "durationMinutes": 175,
  "releaseDate": "2022-06-03",
  "language": "Tamil",
  "genre": "Action"
}
```

**Expected Result:** `201 Created`

Creates a new movie and returns the created movie information.

### 4.2 Get All Movies

**Method:** GET

**Endpoint:**

```http
GET http://localhost:5246/api/movies
```

Retrieves movie records.

**Expected Result:** `200 OK`

### 4.3 Get Movie By ID

**Method:** GET

**Endpoint:**

```http
GET http://localhost:5246/api/movies/1
```

Retrieves a movie using its ID.

**Expected Result:** `200 OK` when the movie exists, or `404 Not Found` when it does not.

### 4.4 Update Movie

**Method:** PUT

**Endpoint:**

```http
PUT http://localhost:5246/api/movies/1
```

**Request Body:**

```json
{
  "title": "Leo - Updated",
  "description": "An updated action thriller movie",
  "durationMinutes": 165,
  "releaseDate": "2023-10-19",
  "language": "Tamil",
  "genre": "Action Thriller"
}
```

**Expected Result:** `204 No Content` when the update succeeds.

### 4.5 Delete Movie

**Method:** DELETE

**Endpoint:**

```http
DELETE http://localhost:5246/api/movies/1
```

Deletes a movie using its ID.

**Expected Result:** `204 No Content` when deletion succeeds.

### 4.6 Search Movies

**Method:** GET

**Endpoint:**

```http
GET http://localhost:5246/api/movies/search?title=Vikram
```

Searches for movies using their title.

**Expected Result:** `200 OK`

The title query parameter is required.

### 4.7 Get Movies with Pagination and Filtering

**Method:** GET

**Endpoint:**

```http
GET http://localhost:5246/api/movies?pageNumber=1&pageSize=10&title=Vikram
```

This endpoint supports pagination and optional title filtering.

**Query Parameters:**

| Parameter | Description |
|---|---|
| pageNumber | Page number |
| pageSize | Number of records per page |
| title | Optional movie title filter |

**Expected Result:** `200 OK`

## 5. HTTP Status Code Validation

| Status Code | Scenario |
|---|---|
| 200 OK | Successful retrieval or search |
| 201 Created | Movie created successfully |
| 204 No Content | Movie updated or deleted successfully |
| 400 Bad Request | Invalid pagination values or missing search title |
| 404 Not Found | Movie does not exist |
| 405 Method Not Allowed | Incorrect HTTP method for an endpoint |

## 6. Testing Observations

During API testing, the following observations were made:

- Movie creation returned `201 Created`.
- The movie retrieval operations were tested.
- Movie update and delete requests were tested.
- Search uses the GET HTTP method.
- The Movies API supports pagination and title filtering.
- Selecting an incorrect HTTP method can result in `405 Method Not Allowed`.

## 7. Conclusion

Postman provides a convenient way to validate the Movie Ticket Booking System APIs. Testing verifies that the endpoints accept the intended HTTP methods, process request data, and return appropriate responses.