using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data.Entities;

[Table("EventCategories")]
public class EventCategoryEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<EventEntity> Events { get; set; } = [];
}
