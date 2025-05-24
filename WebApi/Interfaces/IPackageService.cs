using WebApi.Models;

namespace WebApi.Interfaces;

public interface IPackageService
{
    public Task<ServiceResult<Package>> CreateAsync(PackageForm form);

    public Task<ServiceResult<Package>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<Package>>> GetAllAsync();

    public Task<ServiceResult<Package>> UpdateAsync(int id, PackageForm form);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


