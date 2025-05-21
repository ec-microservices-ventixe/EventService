namespace WebApi.Models;

public class Schedule
{
    public IEnumerable<ScheduleSlot> Slots { get; set; } = [];
}
