using WebApi.Models;

namespace WebApi.Interfaces;

public interface IScheduleService
{
    public Task<ServiceResult<Schedule>> CreateAsync(ScheduleForm form);

    public Task<ServiceResult<Schedule>> GetAsync(int id);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


