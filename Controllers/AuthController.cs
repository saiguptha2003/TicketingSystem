using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using TicketBookingSystem.Models;
using TicketBookingSystem.Dtos;
namespace TicketBookingSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TicketContext _context;

        public AuthController(TicketContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResDTO>> Register([FromBody] RegisterReqDTO userReq)
        {
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = userReq.UserName,
                Email = userReq.Email,
                Password = userReq.HashPassword(),
                PhoneNumber = userReq.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var userRes = new RegisterResDTO
            {
                UserId = user.UserId,
                CreatedAt = user.CreatedAt
            };

            return Created("", userRes);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResDTO>> Login([FromBody] LoginReqDTO userReq)
        {
            var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == userReq.UserName);

            if (user == null)
            {
                return NotFound();
            }
            var userRes=new LoginResDTO{
                UserId=user.UserId,
                UserName=user.UserName,
                Email=user.Email
            };
            return userRes;
        }
    }
}