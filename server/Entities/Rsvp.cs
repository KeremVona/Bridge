using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bridge.Server.Entities.Auth;

[Table("rsvps")]
public class Rsvp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("event_id")]
    public Guid EventId { get; set; }

    [Column("made_at")]
    public DateTime MadeAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(EventId))]
    public Event Event { get; set; } = null!;
}
