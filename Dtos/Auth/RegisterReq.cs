using System.Security.Cryptography;
using System.Text;
namespace TicketBookingSystem.Dtos;
public class RegisterReqDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }

    public string HashPassword()
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(this.Password);
            byte[] hashBytes = sha512.ComputeHash(passwordBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
