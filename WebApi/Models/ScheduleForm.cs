namespace WebApi.Models;

public class ScheduleForm
{
    public IEnumerable<ScheduleSlotForm> Slots { get; set; } = [];
}
