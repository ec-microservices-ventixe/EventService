using WebApi.Models;

namespace WebApi.Interfaces;

public interface IEventService
{
    public Task<ServiceResult<bool>> CreateAsync(EventForm form);

    public Task<ServiceResult<Event>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<Event>>> GetAllAsync();

    public Task<ServiceResult<bool>> UpdateAsync(int id);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


