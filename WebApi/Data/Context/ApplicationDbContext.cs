using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    DbSet<EventEntity> Events { get; set; } = null!;
    DbSet<EventScheduleEntity> EventSchedules { get; set; } = null!;
    DbSet<EventCategoryEntity> EventCategories { get; set; } = null!;

    DbSet<ScheduleSlotEntity> ScheduleSlots { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventEntity>()
        .HasOne(e => e.Category)
        .WithMany( c => c.Events)
        .HasForeignKey(e => e.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventScheduleEntity>()
        .HasOne(s => s.Event)
        .WithOne(e => e.Schedule)
        .HasForeignKey<EventScheduleEntity>(s => s.EventId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
