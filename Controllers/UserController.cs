using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Dtos;
using TicketBookingSystem.Models;

namespace TicketBookingSystem.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TicketContext _context;

        public UserController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            // Retrieve the userId from the JWT token
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID is missing from the token.");
            }

            // Find the user by their userId
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/user
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser(User user)
        {
            // Retrieve the userId from the JWT token
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID is missing from the token.");
            }

            // Ensure the logged-in user is trying to modify their own data
            if (user.UserId != userId)
            {
                return Unauthorized("You are not authorized to modify this user's data.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/user
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            // Retrieve the userId from the JWT token
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID is missing from the token.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("bookings")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderResDTO>>> UserBookings()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID is missing from the token.");
            }

            // Get all orders for the specific user, including related showtimes
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)  // Find all orders for the specific user
                .Include(o => o.Showtime)        // Include related Showtime data
                .ThenInclude(s => s.Movie)       // Include related Movie data
                .ToListAsync();

            // If no orders are found, return a NotFound response
            if (!orders.Any())  // Just check if the list is empty
            {
                return NotFound("No orders found for this user.");
            }

            // Map the orders to OrderResDTOs
            var orderResDTOs = orders.Select(o => new OrderResDTO
            {
                OrderId = o.OrderId,
                ShowTimeId=o.ShowTimeId,
                ShowDate=o.Showtime.ShowDate,
                ShowTime=o.Showtime.ShowTime,
                BookingTime = o.BookingTime,
                TotalAmount = o.TotalAmount,
                SeatsBooked = o.SeatsBooked,
                UpdatedTime=o.UpdatedTime,
            }).ToList();

            // Return the DTOs
            return Ok(orderResDTOs);
        }

        private bool UserExists(string userId)
        {
            return _context.Users.Any(e => e.UserId == userId);
        }
    }
}
