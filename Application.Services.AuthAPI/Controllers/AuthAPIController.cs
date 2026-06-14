using System.Threading.Tasks;
using Application.Services.AuthAPI.Models.Dtos;
using Application.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthAPIController(IAuthService authService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.Success = false;
                _response.Message = errorMessage;
                return Ok(_response);
            }
            _response.Message = "Successfully Registered";
            return Ok(_response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.Success = false;
                _response.Message = "Username or Password is incorrect";
                return Ok(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] OtpRequestDto model)
        {
            var isSent = await _authService.SendOtp(model.PhoneNumber);
            if (!isSent)
            {
                _response.Success = false;
                _response.Message = "Phone number not found or unable to send OTP";
                return Ok(_response);
            }
            _response.Message = "OTP sent successfully";
            return Ok(_response);
        }

        [HttpPost("login-with-otp")]
        public async Task<IActionResult> LoginWithOtp([FromBody] OtpRequestDto model)
        {
            var loginResponse = await _authService.LoginWithOtp(model);
            if (loginResponse.User == null)
            {
                _response.Success = false;
                _response.Message = "Invalid OTP";
                return Ok(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole([FromBody] Dictionary<string, string> model)
        {
            if (model.TryGetValue("Email", out var email) && model.TryGetValue("Role", out var role))
            {
                var assignRoleSuccessful = await _authService.AssignRole(email, role.ToUpper());
                if (!assignRoleSuccessful)
                {
                    _response.Success = false;
                    _response.Message = "Unable to assign the role";
                    return Ok(_response);
                }
                _response.Message = "Successfully Assigned";
            }
            else
            {
                _response.Success = false;
                _response.Message = "Email or Role is missing";
                return BadRequest(_response);
            }

            return Ok(_response);
        }


        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken()
        {
            
            // Retrieve the token from the Authorization header
            var userIdHeader = Request.Headers["UserId"].ToString();
            if (string.IsNullOrEmpty(userIdHeader) || !Guid.TryParse(userIdHeader, out var UserId))
            {
                _response.Success = false;
                _response.Message = "User ID is missing or invalid";
                return BadRequest(_response);
            }
            
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                _response.Success = false;
                _response.Message = "Authorization header is missing or invalid";
                return BadRequest(_response);
            }
            
            var user = await _authService.ValidateToken(token, UserId);
            if (user == null)
            {
                _response.Success = false;
                _response.Message = "Invalid token";
                return Ok(_response);
            }
            _response.Result = user;
            _response.Message = "Token is valid";
            return Ok(_response);
        }

        [HttpPost("getusers")]
        public async Task<IActionResult> GetUsers([FromBody] string[]? roles)
        {
            var users = await _authService.GetUsers(roles);
            if (users == null || !users.Any())
            {
                _response.Success = false;
                _response.Message = "No users found";
                return Ok(_response);
            }
            _response.Result = users;
            return Ok(_response);
        }

        [HttpGet("getuser/{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _authService.GetUser(userId);
            if (user == null)
            {
                _response.Success = false;
                _response.Message = "User not found";
                return Ok(_response);
            }
            _response.Result = user;
            return Ok(_response);
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto model)
        {
            var errorMessage = await _authService.UpdateUser(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.Success = false;
                _response.Message = errorMessage;
                return Ok(_response);
            }
            _response.Result = model.Id;
            _response.Message = "Successfully Updated User";
            return Ok(_response);
        }
    }
}
