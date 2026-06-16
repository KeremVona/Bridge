namespace Bridge.Server;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto?> LoginAsync(LoginDto request);
}
