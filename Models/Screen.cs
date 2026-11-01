
namespace TicketBookingApplication.Models;

public class Screen
{
    public int ScreenId { get; set; }

    public int TheatreId { get; set; }

    public string ScreenName { get; set; } = string.Empty;

    public int TotalSeats { get; set; }

    // Navigation properties
    public Theatre? Theatre { get; set; }
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public ICollection<Show> Shows { get; set; } = new List<Show>();
}

}
