using WebApi.Models;

namespace WebApi.Interfaces;

public interface IEventService
{
    public Task<ServiceResult<Event>> CreateAsync(EventForm form);

    public Task<ServiceResult<Event>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<Event>>> GetAllAsync();

    public Task<ServiceResult<Event>> UpdateAsync(int id, EventForm form);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


