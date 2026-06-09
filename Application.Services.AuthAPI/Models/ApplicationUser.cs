using Microsoft.AspNetCore.Identity;

namespace Application.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiryTime { get; set; }
    }
}
