using System.ComponentModel.DataAnnotations;

namespace Bridge.Server;

public record RegisterDto(
    [Required] [EmailAddress] string Email,
    [Required] [MinLength(6)] string Password,
    [Required] string FullName,
    [Required] string HomeCity,
    [Required] string CurrentCity
);
