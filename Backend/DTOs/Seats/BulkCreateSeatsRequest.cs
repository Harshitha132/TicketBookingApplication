
using System.ComponentModel.DataAnnotations;

namespace TicketBookingApplication.DTOs.Seats;

public class BulkCreateSeatsRequest
{
    [Range(1, int.MaxValue)]
    public int ScreenId { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateSeatRequest> Seats { get; set; } = new();
}
