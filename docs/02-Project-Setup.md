# Project Setup and Configuration

## 1. Project Creation

The Movie Ticket Booking System is developed as a single ASP.NET Core Web API project.

**Project Name:** TicketBookingApplication  
**Framework:** .NET 10  
**Language:** C#

The project was created using the .NET CLI.

```powershell
dotnet new webapi -n TicketBookingApplication
cd TicketBookingApplication
```

## 2. Development Environment

The following tools are used during development:

- Visual Studio / Visual Studio Code
- .NET SDK 10
- SQL Server Express
- SQL Server Management Studio (SSMS)
- Postman
- Git and GitHub

## 3. Required NuGet Packages

The following packages are used in the project:

| Package | Version | Purpose |
|---|---|---|
| Microsoft.EntityFrameworkCore.Design | 10.0.14 | EF Core design-time operations |
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.14 | SQL Server database provider |
| Microsoft.EntityFrameworkCore.Tools | 10.0.14 | Migration and database management tools |
| Scalar.AspNetCore | 2.18.5 | API documentation support |

### Package Installation Commands

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.14

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.14

dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.14

dotnet add package Scalar.AspNetCore --version 2.18.5
```

## 4. Database Configuration

SQL Server Express is used as the relational database.

**Database Name:** MovieTicketBookingDB

The connection string is configured in `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Connection String Explanation

- `Server=.\\SQLEXPRESS` – Connects to the local SQL Server Express instance.
- `Database=MovieTicketBookingDB` – Specifies the database name.
- `Trusted_Connection=True` – Uses Windows authentication.
- `TrustServerCertificate=True` – Trusts the SQL Server certificate for the connection.

## 5. Entity Framework Core Configuration

Entity Framework Core is configured in `Program.cs` using dependency injection.

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

This registers `ApplicationDbContext` and configures SQL Server as the database provider.

## 6. Database Migration Setup

EF Core migrations are used to create and update the database schema.

The initial migration was created using:

```powershell
dotnet ef migrations add InitialCreate
```

The database was created and updated using:

```powershell
dotnet ef database update
```

The migration files are maintained in the `Migrations` folder.

## 7. Running the Application

Run the following command from the project directory:

```powershell
dotnet run
```

The application runs using the configured HTTP and HTTPS development URLs.

The HTTP URL used during Postman testing is:

```text
http://localhost:5246
```

## 8. API Testing Setup

Postman is used to send HTTP requests to the Web API.

The base URL used for testing is:

```text
http://localhost:5246/api
```

Requests are organized into a Postman collection named:

**Movie Ticket Booking System**

The collection contains separate folders for individual modules, beginning with the Movies module.

## 9. Conclusion

The project environment is configured using .NET 10, Entity Framework Core 10, and SQL Server Express. Database connectivity, dependency injection, and EF Core migrations are configured to support the development of the Movie Ticket Booking System.