

namespace TicketBookingSystem.Dtos;

public class OrderResDTO{
    public string OrderId {get; set;}
    public string  ShowTimeId { get; set; }
    public DateOnly ShowDate { get; set; }  
    public TimeOnly ShowTime { get; set; }  
    public DateTime BookingTime {get; set;}
    public required long SeatsBooked { get; set; }
    public DateTime UpdatedTime { get; set; }
    public required decimal TotalAmount { get; set; }
    }