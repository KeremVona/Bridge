using Bridge.Server.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // 1. Add DbSets for your entities
    public DbSet<User> Users { get; set; }

    // Assuming these exist based on the navigation properties in your User entity
    public DbSet<Event> Events { get; set; }
    public DbSet<Rsvp> Rsvps { get; set; }

    // 2. Configure database constraints and relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Unique constraint for RSVPs (User can only RSVP once per event)
        modelBuilder.Entity<Rsvp>().HasIndex(r => new { r.UserId, r.EventId }).IsUnique();

        // Configure Performance Indexes
        modelBuilder.Entity<User>().HasIndex(u => u.CurrentCity);
        modelBuilder.Entity<Event>().HasIndex(e => e.City);
        modelBuilder.Entity<Event>().HasIndex(e => e.TargetHomeCity);

        // Configure explicit relationships and cascade rules
        modelBuilder
            .Entity<Event>()
            .HasOne(e => e.Organizer)
            .WithMany(u => u.OrganizedEvents)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Rsvp>()
            .HasOne(r => r.User)
            .WithMany(u => u.Rsvps)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Rsvp>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Rsvps)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
