namespace TicketBookingSystem.Dtos;
public class BookingTicketResDTO{
     public string? OrderId { get; set; } 
     public required DateTime BookingTime { get; set; }
     public required decimal TotalAmount { get; set; }
    public required long SeatsBooked { get; set; }
    public DateTime UpdatedTime { get; set; }

    public MovieDetails ShowDetails {get; set;}
}

public class MovieDetails{
        public required string TheaterName { get; set; }
        public required string Location { get; set; }
        public string? MovieTitle { get; set; }
        public required DateOnly ShowDate { get; set; }
        public required TimeOnly ShowTime { get; set; }
        

}