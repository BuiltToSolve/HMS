namespace Application.Services.AuthAPI.Models.Dtos
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
        public string Message { get; set; }
    }
}
