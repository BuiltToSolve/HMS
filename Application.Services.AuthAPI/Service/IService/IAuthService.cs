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
        Task<List<UserDto>> GetUsers(string[]? roles);
        Task<UserDto> GetUser(Guid userId);
        Task<string?> UpdateUser(UserDto userDto);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> SendOtp(string phoneNumber);
        Task<LoginResponseDto> LoginWithOtp(OtpRequestDto otpRequestDto);
    }
}
