using TicketBookingApplication.DTOs.Auth;

namespace TicketBookingApplication.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}