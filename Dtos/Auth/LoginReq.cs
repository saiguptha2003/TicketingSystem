
using System.Security.Cryptography;
using System.Text;
namespace TicketBookingSystem.Dtos;
public class LoginReqDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public bool VerifyPassword(string storedHash)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(this.Password);
            byte[] hashBytes = sha512.ComputeHash(passwordBytes);
            string hashedEnteredPassword = Convert.ToHexString(hashBytes);

            // Compare the newly hashed password with the stored hash
            return hashedEnteredPassword == storedHash;
        }
    }
}