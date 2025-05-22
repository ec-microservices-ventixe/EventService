using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Data.Entities;

[Table("ScheduleSlots")]
public class ScheduleSlotEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Name { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int ScheduleId { get; set; }

    public EventScheduleEntity Schedule { get; set; } = null!;
}
