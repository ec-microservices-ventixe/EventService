using WebApi.Models;

namespace WebApi.Interfaces;

public interface ICategoryService
{
    public Task<ServiceResult<bool>> CreateAsync(CategoryForm form);

    public Task<ServiceResult<Category>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<Category>>> GetAllAsync();

    public Task<ServiceResult<bool>> UpdateAsync(int id);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


