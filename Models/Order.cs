namespace TicketBookingSystem.Models
{
    public class Order
    {
        public string? OrderId { get; set; }

        // Foreign Key to User
        public required string? UserId { get; set; }
        public User? User { get; set; } // Navigation property to User

        // Foreign Key to Showtime
        public required string? ShowTimeId { get; set; }
        public Showtime? Showtime { get; set; } // Navigation property to Showtime

        public required decimal TotalAmount { get; set; }
        public required DateTime BookingTime { get; set; }
        public STATUS Status { get; set; } // Status is an enum (Booked, Cancelled)
        public required long SeatsBooked { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}