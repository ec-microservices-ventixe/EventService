using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

public class EventEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(500)")]
    public string Description { get; set; } = null!;
}
