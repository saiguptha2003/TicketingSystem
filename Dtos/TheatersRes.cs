public class TheatersResDTO{
        public string? TheaterId { get; set; }
        public required string TheaterName { get; set; }
        public required string Location { get; set; }
        public long TotalSeats { get; set; }
        public required List<MovieTD>? Movies { get; set; }
}


public class MovieTD{
    public required string? MovieId { get; set; }
    public string? Title { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public required List<ShowTD>? Showtimes { get; set; }
}
public class ShowTD{
    public string? ShowTimeId {get; set;}
    public DateOnly ShowDate {get; set;}
    public long AvailableSeats {get;set;}
    public DateTime UpdatedTime {get; set;}
    public required TimeOnly ShowTime { get; set; }


}