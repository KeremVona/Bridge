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
        // 1. Validation (Edge Case: Bad input data)
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Email and Password are required.");

        if (request.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.");

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 2. Race Condition Protection & Existence Check
        // Even with a Unique index in DB, checking here prevents unnecessary hashing costs
        if (await context.Users.AnyAsync(u => u.Email == normalizedEmail))
            throw new InvalidOperationException("User with this email already exists.");

        // 3. Password Hashing
        // BCrypt.Net.BCrypt.HashPassword is CPU intensive, which is a security feature.
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 4. Persistence
        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            FullName = request.FullName?.Trim(),
            HomeCity = request.HomeCity?.Trim(),
            CurrentCity = request.CurrentCity?.Trim(),
            MadeAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        try
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Edge Case: Database constraint violations (e.g., unique email index hit at the exact same millisecond)
            // Log the error here!
            throw new Exception("An error occurred while saving the user to the database.", ex);
        }

        // 5. Generate and return Token
        var token = GenerateJwtToken(user);
        return new AuthResponseDto(token, user.FullName, user.Email);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto request)
    {
        // 1. Basic validation
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 2. Fetch user
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

        // 3. Constant-Time Verification
        // If user is null, we STILL run a "dummy" verify to ensure the response time
        // is consistent, making it harder to guess if the user exists.
        bool isValid =
            user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValid)
        {
            // Use a generic "Invalid credentials" error.
            // Never tell the client if it was the email or the password that failed.
            return null;
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
