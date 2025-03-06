namespace TicketBookingSystem.Models
{
    public class Movie
    {
        public long MovieId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal Rating { get; set; }
        public required List<Showtime> Showtimes { get; set; }
    }
}