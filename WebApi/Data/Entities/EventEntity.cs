using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

[Table("Events")]
public class EventEntity
{
    [Key]
    public int Id { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(500)")]
    public string Description { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(250)")]

    public string Location { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TotalTickets { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public int? ScheduleId { get; set; }

    public EventScheduleEntity? Schedule { get; set; }

    public int? CategoryId { get; set; }

    public EventCategoryEntity? Category { get; set; } 

    
}
