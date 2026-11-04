using Microsoft.AspNetCore.Identity;

namespace TicketBookingApplication.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}