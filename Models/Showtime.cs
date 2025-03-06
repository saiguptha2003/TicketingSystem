namespace TicketBookingSystem.Models
{
    public class Showtime
    {
        public long ShowTimeId { get; set; }

        public required long MovieId { get; set; }
        public Movie Movie { get; set; }

        public required long TheaterId { get; set; }
        public Theater Theater { get; set; }

        public required DateOnly ShowDate { get; set; }
        public required TimeOnly ShowTime { get; set; }
        public required long AvailableSeats { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}