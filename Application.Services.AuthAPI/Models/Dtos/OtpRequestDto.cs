namespace Application.Services.AuthAPI.Models.Dtos
{
    public class OtpRequestDto
    {
        public string PhoneNumber { get; set; }
        public string? Otp { get; set; }
    }
}
