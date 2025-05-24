using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Data.Entities;

[Table("EventPackages")]
public class EventPackageEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "varchar(50)")] 
    public string Name { get; set; } = null!;

    public bool? IsSeating { get; set; }

    [Column(TypeName = "varchar(120)")]

    public string Benefits { get; set; } = null!;

    [Required]
    public float ExtraFeeInProcent { get; set; }

    [Required]
    public int NumberOfTickets { get; set; }

    public int EventId { get; set; }

    public EventEntity Event { get; set; } = null!;
}
