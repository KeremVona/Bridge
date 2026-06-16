using Bridge.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                var result = await authService.RegisterAsync(request);
                return Ok(
                    new ApiResponse<AuthResponseDto>(true, "User registered successfully", result)
                );
            }
            catch (InvalidOperationException ex)
            {
                // Returns 409 Conflict if user exists
                return Conflict(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await authService.LoginAsync(request);

            // Keep it generic to prevent account enumeration
            if (result == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid email or password."));

            return Ok(new ApiResponse<AuthResponseDto>(true, "Login successful", result));
        }
    }
}
