using Bridge.Server.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("events")]
public class Event
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("id")]
  public Guid Id { get; set; }

  [Required]
  [MaxLength(150)]
  [Column("title")]
  public string Title { get; set; } = string.Empty;

  [Required]
  [Column("description")]
  public string Description { get; set; } = string.Empty;

  [Required]
  [MaxLength(255)]
  [Column("location")]
  public string Location { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  [Column("city")]
  public string City { get; set; } = string.Empty;

  [MaxLength(100)]
  [Column("target_home_city")]
  public string? TargetHomeCity { get; set; }

  [Column("is_periodic")]
  public bool IsPeriodic { get; set; } = false;

  [Required]
  [Column("start_time")]
  public DateTime StartTime { get; set; }

  [Column("recurrence_rule")]
  public string? RecurrenceRule { get; set; }

  [Required]
  [Column("organizer_id")]
  public Guid OrganizerId { get; set; }

  [Column("made_at")]
  public DateTime MadeAt { get; set; } = DateTime.UtcNow;

  [Column("updated_at")]
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // Navigation Properties
  [ForeignKey(nameof(OrganizerId))]
  public User Organizer { get; set; } = null!;
  public ICollection<Rsvp> Rsvps { get; set; } = new List<Rsvp>();
}
