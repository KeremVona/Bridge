using System.ComponentModel.DataAnnotations;

namespace Bridge.Server;

public record LoginDto([Required] [EmailAddress] string Email, [Required] string Password);
