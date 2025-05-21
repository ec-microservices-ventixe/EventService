using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

[Table("EventSchedules")]
public class EventScheduleEntity
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(EventId))]
    public EventEntity Event { get; set; } = null!;
    public int EventId { get; set; }

    public ICollection<ScheduleSlotEntity> ScheduleSlots { get; set; } = [];
}
