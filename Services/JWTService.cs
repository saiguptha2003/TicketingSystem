using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace TicketBookingSystem.Services;

public class JWT
{
    public static string GenerateJwtToken(string username, string userId)
    {
        var claims = new[] 
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", userId) // Add userId as a claim
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("lYifrI8MCHI9CWYWphTT3wiFpiPvh6AD3EcorB4k9jxKktLGursWeM4AdTN0UoayAlDuj0NpE8FGBW38cpKZXavv75Oj8ZffvNI3FAN0poxPmGsPJdptwBQJkn6Ym5kt"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
