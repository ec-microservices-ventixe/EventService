using WebApi.Models;

namespace WebApi.Interfaces;

public interface IScheduleSlotService
{
    public Task<ServiceResult<ScheduleSlot>> CreateAsync(ScheduleSlotForm form);

    public Task<ServiceResult<ScheduleSlot>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<ScheduleSlot>>> GetAllAsync();

    public Task<ServiceResult<ScheduleSlot>> UpdateAsync(int id, ScheduleSlotForm form);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


