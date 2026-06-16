using Bridge.Server;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await authService.RegisterAsync(request);

            if (result == null)
                return BadRequest(new { Message = "Email is already in use." });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await authService.LoginAsync(request);

            if (result == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            return Ok(result);
        }
    }
}
