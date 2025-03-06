namespace TicketBookingSystem.Models
{
    public class User
    {
        public long UserId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property for orders
        public ICollection<Order> Orders { get; set; }
    }
}