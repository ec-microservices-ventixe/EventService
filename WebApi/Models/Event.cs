namespace WebApi.Models;

public class Event
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TotalTickets { get; set; }

    public decimal Price { get; set; }

    public Category? Category { get; set; }

    public Schedule? Schedule { get; set; }
}
