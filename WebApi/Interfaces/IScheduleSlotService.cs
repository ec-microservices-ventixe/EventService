using WebApi.Models;

namespace WebApi.Interfaces;

public interface IScheduleSlotService
{
    public Task<ServiceResult<bool>> CreateAsync(ScheduleSlotForm form);

    public Task<ServiceResult<ScheduleSlot>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<ScheduleSlot>>> GetAllAsync();

    public Task<ServiceResult<bool>> UpdateAsync(int id);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


