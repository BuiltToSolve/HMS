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

        public async Task<List<UserDto>> GetUsers(string[]? roles)
        {
            try
            {
                List<ApplicationUser> matchedUsers;

                if (roles == null || roles.Length == 0 || roles.All(string.IsNullOrEmpty))
                {
                    matchedUsers = _db.ApplicationUsers.ToList();
                }
                else
                {
                    var usersList = new List<ApplicationUser>();
                    foreach (var role in roles)
                    {
                        if (string.IsNullOrEmpty(role)) continue;

                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                            usersList.AddRange(usersInRole);
                        }
                    }
                    matchedUsers = usersList.GroupBy(u => u.Id).Select(g => g.First()).ToList();
                }

                // Fetch roles for all matched users
                var matchedUserIds = matchedUsers.Select(u => u.Id).ToList();
                var userRoles = _db.UserRoles
                    .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                    .AsEnumerable() // Pull join into memory to avoid SQL Server compatibility level / OPENJSON issues
                    .Where(ur => matchedUserIds.Contains(ur.UserId))
                    .ToList();

                var rolesByUser = userRoles
                    .GroupBy(ur => ur.UserId)
                    .ToDictionary(g => g.Key, g => (IList<string>)g.Select(ur => ur.Name).ToList());

                var userDtos = matchedUsers.Select(user =>
                {
                    var dto = _mapper.Map<UserDto>(user);
                    if (rolesByUser.TryGetValue(user.Id, out var rolesList))
                    {
                        dto.Roles = rolesList;
                    }
                    else
                    {
                        dto.Roles = new List<string>();
                    }
                    return dto;
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Log the exception if needed
                return new List<UserDto>();
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Username or Password is incorrect" };
            }
            if (!user.IsActive)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Account has been suspended. Please contact support" };
            }
            if (!isValid)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Username or Password is incorrect" };
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
                PhoneNumber = registrationRequestDto.PhoneNumber,
                IsActive = registrationRequestDto.IsActive
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    if (registrationRequestDto.Roles != null && registrationRequestDto.Roles.Length > 0)
                    {
                        foreach (var role in registrationRequestDto.Roles)
                        {
                            if (!string.IsNullOrEmpty(role))
                            {
                                await AssignRole(user.Email, role.ToUpper());
                            }
                        }
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
                if (user == null || !user.IsActive)
                {
                    return null;
                }
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
            
            if (user == null)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Invalid OTP" };
            }
            
            if (!user.IsActive)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Account has been suspended. Please contact support" };
            }

            // Validate OTP value
            if (user.Otp != otpRequestDto.Otp)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "Invalid OTP" };
            }

            // Validate expiration date
            if (user.OtpExpiryTime < DateTime.UtcNow)
            {
                return new LoginResponseDto { User = null, Token = "", Message = "OTP has expired" };
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

        public async Task<string?> UpdateUser(UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);
                if (user == null)
                {
                    return "User not found";
                }

                user.Name = userDto.Name;
                user.Email = userDto.Email;
                user.NormalizedEmail = userDto.Email?.ToUpper();
                user.UserName = userDto.Email;
                user.NormalizedUserName = userDto.Email?.ToUpper();
                user.PhoneNumber = userDto.PhoneNumber;
                user.IsActive = userDto.IsActive;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to update user properties";
                }

                if (userDto.Roles != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var newRoles = userDto.Roles.Where(r => !string.IsNullOrEmpty(r)).Select(r => r.ToUpper()).ToList();
                    var currentRolesUpper = currentRoles.Select(r => r.ToUpper()).ToList();

                    var rolesToRemove = currentRoles.Where(r => !newRoles.Contains(r.ToUpper())).ToList();
                    var rolesToAdd = userDto.Roles.Where(r => !string.IsNullOrEmpty(r) && !currentRolesUpper.Contains(r.ToUpper())).ToList();

                    if (rolesToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        if (!removeResult.Succeeded)
                        {
                            return removeResult.Errors.FirstOrDefault()?.Description ?? "Failed to remove old roles";
                        }
                    }

                    if (rolesToAdd.Any())
                    {
                        foreach (var role in rolesToAdd)
                        {
                            var roleUpper = role.ToUpper();
                            if (!await _roleManager.RoleExistsAsync(roleUpper))
                            {
                                await _roleManager.CreateAsync(new IdentityRole(roleUpper));
                            }
                            var addResult = await _userManager.AddToRoleAsync(user, roleUpper);
                            if (!addResult.Succeeded)
                            {
                                return addResult.Errors.FirstOrDefault()?.Description ?? $"Failed to add role {role}";
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
