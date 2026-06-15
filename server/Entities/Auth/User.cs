namespace Bridge.Server.Entities.Auth;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("id")]
  public Guid Id { get; set; }

  [Required]
  [EmailAddress]
  [MaxLength(255)]
  [Column("email")]
  public string Email { get; set; } = string.Empty;

  [Required]
  [MaxLength(255)]
  [Column("password_hash")]
  public string PasswordHash { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  [Column("full_name")]
  public string FullName { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  [Column("home_city")]
  public string HomeCity { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  [Column("current_city")]
  public string CurrentCity { get; set; } = string.Empty;

  [Column("made_at")]
  public DateTime MadeAt { get; set; } = DateTime.UtcNow;

  [Column("updated_at")]
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // Navigation Properties
  public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
  public ICollection<Rsvp> Rsvps { get; set; } = new List<Rsvp>();
}
