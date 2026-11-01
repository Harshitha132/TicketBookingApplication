
using System.ComponentModel.DataAnnotations;

namespace TicketBookingApplication.DTOs.Screens;

public class CreateScreenRequest
{
    [Required]
    public int TheatreId { get; set; }

    [Required]
    [StringLength(100)]
    public string ScreenName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int TotalSeats { get; set; }
}
