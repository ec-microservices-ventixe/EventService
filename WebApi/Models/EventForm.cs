namespace WebApi.Models;

public class EventForm
{
    public IFormFile? EventImageFile { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TotalTickets { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; } 

    public int ScheduleId { get; set; }
}
