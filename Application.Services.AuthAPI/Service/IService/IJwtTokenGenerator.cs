using System.Security.Claims;
using Application.Services.AuthAPI.Models;

namespace Application.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(List<Claim> claims);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
