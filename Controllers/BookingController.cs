using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Models;
using TicketBookingSystem.Dtos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace TicketBookingSystem.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly TicketContext _context;

        public BookingController(TicketContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<MoviesResDTO>>> GetMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            var moviesDTO = new List<MoviesResDTO>();
            foreach (var m in movies)
            {
                var md = new MoviesResDTO
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    Genre = m.Genre,
                    Price = m.Price,
                    CreatedAt = m.CreateAt

                };
                moviesDTO.Add(md);
            }
            return (moviesDTO);
        }
        [HttpGet("{id}/showDetails")]
        public async Task<ActionResult<IEnumerable<ShowListResDTO>>> GetShowDetails(string id)
        {
            var showtimes = await _context.Showtimes
                              .Where(s => s.MovieId == id)
                              .Include(s => s.Movie).Include(s => s.Theater)
                              .ToListAsync();
            var ShowDTO = new List<ShowListResDTO>();
            foreach (var s in showtimes)
            {
                var show = new ShowListResDTO
                {
                    ShowTimeId = s.ShowTimeId,
                    TheaterId = s.Theater.TheaterId,
                    TheaterName = s.Theater.TheaterName,
                    ShowDate = s.ShowDate,
                    ShowTime = s.ShowTime,
                    AvailableSeats = s.AvailableSeats,
                    UpdatedTime = s.UpdatedTime,
                    Location = s.Theater.Location

                };
                ShowDTO.Add(show);
            }
            return ShowDTO;
        }
        [HttpGet("/Theaters")]
        public async Task<ActionResult<IEnumerable<TheatersResDTO>>> GetTheaters()
        {
            // Query theaters and include showtimes and related movies
            var theaters = await _context.Theaters
                                          .Include(t => t.Showtimes) // Include Showtimes for each Theater
                                          .ThenInclude(s => s.Movie) // Include Movie for each Showtime
                                          .ToListAsync();

            // Map the data into the DTOs
            var theatersRes = theaters.Select(t => new TheatersResDTO
            {
                TheaterId = t.TheaterId,
                TheaterName = t.TheaterName,
                Location = t.Location,
                TotalSeats = t.TotalSeats,
                Movies = t.Showtimes?.GroupBy(s => s.Movie)
                    .Select(m => new MovieTD
                    {
                        MovieId = m.Key.MovieId,
                        Title = m.Key.Title,
                        ReleaseDate = m.Key.ReleaseDate,
                        Price = m.Key.Price,
                        Showtimes = m.Select(s => new ShowTD
                        {
                            ShowTimeId = s.ShowTimeId,
                            ShowDate = s.ShowDate,
                            AvailableSeats = s.AvailableSeats,
                            ShowTime = s.ShowTime,
                            UpdatedTime = s.UpdatedTime
                        }).ToList()
                    }).ToList()
            }).ToList();

            return theatersRes;
        }
        [HttpPost("Book")]
        [Authorize]
        public async Task<ActionResult<BookingTicketResDTO>> bookMovieTicket(BookTicketReqDTO booking)
        {
            // Find the showtime with the given MovieId and ShowTimeId, including related Movie and Theater data
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)  // Ensure Movie is included
                .Include(s => s.Theater) // Ensure Theater is included
                .FirstOrDefaultAsync(s => s.MovieId == booking.MovieId && s.ShowTimeId == booking.ShowTimeId);

            // If no showtime is found or there are no available seats, return an error response
            if (showtime == null)
            {
                return NotFound("Showtime not found.");
            }

            if (showtime.AvailableSeats < booking.TotalNumberOfTickets)
            {
                return BadRequest("Not enough available seats.");
            }

            // Create a new order
            var order = new Order
            {
                UserId = User.FindFirst("userId")?.Value,
                OrderId = Guid.NewGuid().ToString(), // Generate a new Order ID
                BookingTime = DateTime.Now,
                TotalAmount = showtime.Movie.Price * booking.TotalNumberOfTickets,
                SeatsBooked = booking.TotalNumberOfTickets,
                ShowTimeId = showtime.ShowTimeId, // Linking to the ShowTime
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            };

            // Add the order to the database
            _context.Orders.Add(order);

            // Update the available seats in the ShowTime
            showtime.AvailableSeats -= booking.TotalNumberOfTickets;
            showtime.UpdatedTime = DateTime.Now; // Update the time when the showtime is modified

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Map the order details to BookingTicketResDTO
            var response = new BookingTicketResDTO
            {
                OrderId = order.OrderId,
                BookingTime = order.BookingTime,
                TotalAmount = order.TotalAmount,
                SeatsBooked = order.SeatsBooked,
                UpdatedTime = order.UpdatedTime,
                ShowDetails = new MovieDetails
                {
                    TheaterName = showtime.Theater?.TheaterName,  // Null check on Theater
                    Location = showtime.Theater?.Location,        // Null check on Theater
                    MovieTitle = showtime.Movie?.Title,           // Null check on Movie
                    ShowDate = showtime.ShowDate,
                    ShowTime = showtime.ShowTime
                }
            };

            return response;
        }



    }
}
