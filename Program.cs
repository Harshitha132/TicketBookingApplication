
using Microsoft.EntityFrameworkCore;
using TicketBookingApplication.Data;
using TicketBookingApplication.Repositories;
using TicketBookingApplication.Repositories.Interfaces;
using TicketBookingApplication.Services;
using TicketBookingApplication.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Register Service
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
