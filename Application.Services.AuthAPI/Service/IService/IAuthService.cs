using System.Security.Claims;
using Application.Services.AuthAPI.Models.Dtos;

namespace Application.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<LoginResponseDto> ValidateToken(string token, Guid UserId);
        Task<List<UserDto>> GetUsers(string roleName);
        Task<UserDto> GetUser(Guid userId);
        Task<bool> SendOtp(string phoneNumber);
        Task<LoginResponseDto> LoginWithOtp(OtpRequestDto otpRequestDto);
    }
}
