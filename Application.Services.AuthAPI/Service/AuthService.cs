using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services.AuthAPI.Database;
using Application.Services.AuthAPI.Models;
using Application.Services.AuthAPI.Models.Dtos;
using Application.Services.AuthAPI.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
                if (user != null)
                {
                    if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    }
                    await _userManager.AddToRoleAsync(user, roleName);
                    return true;
                }
            }
            return false;
        }

        public Task<UserDto> GetUser(Guid userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId.ToString());
            if (user != null)
            {
                return Task.FromResult(_mapper.Map<UserDto>(user));
            }
            return Task.FromResult<UserDto>(null);
        }

        public async Task<List<UserDto>> GetUsers(string roleName)
        {
            try
            {
                // Check if the role exists
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    return new List<UserDto>();
                }

                // Get users in the specified role
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

                // Map the users to UserDto
                var userDtos = usersInRole.Select(user => _mapper.Map<UserDto>(user)).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return new List<UserDto>();
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null || isValid == false)
            {
                return new LoginResponseDto { User = null, Token = "" };
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            };
            var token = _jwtTokenGenerator.GenerateToken(claims);

            LoginResponseDto loginResponseDto = new()
            {
                User = _mapper.Map<UserDto>(user),
                Token = token,
                Roles = roles
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                Name = registrationRequestDto.Name,
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registrationRequestDto.Role))
                    {
                        await AssignRole(user.Email, registrationRequestDto.Role.ToUpper());
                    }
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<LoginResponseDto> ValidateToken(string token, Guid UserId)
        {
            try
            {
                var principal = _jwtTokenGenerator.ValidateToken(token);
                if (principal == null)
                {
                    return null;
                }
                //var userId =  principal.FindFirst("sub");         
                //var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);

                LoginResponseDto loginResponseDto = new()
                {
                    User = _mapper.Map<UserDto>(user),
                    Token = token,
                    Roles = roles,
                };
                return loginResponseDto;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> SendOtp(string phoneNumber)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return false;
            }

            // Generate a 4-digit OTP
            var otp = new Random().Next(1000, 9999).ToString();
            user.Otp = otp;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5); // standard expiration


            await _db.SaveChangesAsync();

            // For now, OTP is just saved to DB as requested.
            // SMS gateway integration would go here.
            return true;
        }

        public async Task<LoginResponseDto> LoginWithOtp(OtpRequestDto otpRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == otpRequestDto.PhoneNumber);
            
            // Validate user, OTP value, and expiration date
            if (user == null || user.Otp != otpRequestDto.Otp || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return new LoginResponseDto { User = null, Token = "" };
            }

            // Clear the OTP upon successful login
            user.Otp = null;
            user.OtpExpiryTime = null;
            await _db.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
            };
            var token = _jwtTokenGenerator.GenerateToken(claims);

            LoginResponseDto loginResponseDto = new()
            {
                User = _mapper.Map<UserDto>(user),
                Token = token,
                Roles = roles
            };

            return loginResponseDto;
        }
    }
}
