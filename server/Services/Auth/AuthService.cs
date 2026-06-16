using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bridge.Server.Data;
using Bridge.Server.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bridge.Server.Services.Auth;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto request)
    {
        // 1. Normalize email casing to prevent login mismatch bugs later
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 2. Check if user exists
        if (await context.Users.AnyAsync(u => u.Email == normalizedEmail))
        {
            return null; // Or throw a custom exception
        }

        // 3. Hash Password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 4. Make User Entity
        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            FullName = request.FullName,
            HomeCity = request.HomeCity,
            CurrentCity = request.CurrentCity,
            MadeAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // 5. Generate and return Token
        var token = GenerateJwtToken(user);
        return new AuthResponseDto(token, user.FullName, user.Email);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null; // Invalid credentials
        }

        var token = GenerateJwtToken(user);
        return new AuthResponseDto(token, user.FullName, user.Email);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("FullName", user.FullName),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
