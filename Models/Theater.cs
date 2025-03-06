namespace TicketBookingSystem.Models
{
    public class Theater
    {
        public string? TheaterId { get; set; }
        public required string TheaterName { get; set; }
        public required string Location { get; set; }
        public long TotalSeats { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public ICollection<Showtime>? Showtimes { get; set; }
    }
}