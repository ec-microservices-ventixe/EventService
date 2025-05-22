namespace WebApi.Models;

public class Schedule
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public IEnumerable<ScheduleSlot> ScheduleSlots { get; set; } = [];
}
