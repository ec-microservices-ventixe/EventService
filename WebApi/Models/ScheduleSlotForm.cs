namespace WebApi.Models;

public class ScheduleSlotForm
{
    public int EventId { get; set; }
    public string Name { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}