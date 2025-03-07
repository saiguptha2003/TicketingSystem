public class ShowListResDTO{
    public string? ShowTimeId {get; set;}
    public string? TheaterId {get; set;}
    public string? TheaterName { get; set; }
    public DateOnly ShowDate {get; set;}
    public TimeOnly ShowTime {get; set;}
    public long AvailableSeats {get;set;}
    public DateTime UpdatedTime {get; set;}
    public string? Location {get; set;}

}