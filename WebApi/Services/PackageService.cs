using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class PackageService(IEventPackageRepository eventPackageRepository, IEventRepository eventRepository) : IPackageService
{
    private readonly IEventPackageRepository _packageRepository = eventPackageRepository;
    private readonly IEventRepository _eventRepository = eventRepository;
    public async Task<ServiceResult<Package>> CreateAsync(PackageForm form)
    {
        try
        {
            var eventEntity = await _eventRepository.GetAsync(findBy: x => x.Id == form.EventId);
            if (eventEntity is null) return ServiceResult<Package>.BadRequest("Event does not exist");

            var createdEntity = await _packageRepository.CreateAsync(form.MapTo<EventPackageEntity>());
            if (createdEntity is null) return ServiceResult<Package>.Error("Failed to create package");

            return ServiceResult<Package>.Ok(createdEntity.MapTo<Package>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Package>.Error("Failed to create package");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var exisitingEntity = await _packageRepository.GetAsync(findBy: x => x.Id == id);
            if (exisitingEntity is null) return ServiceResult<bool>.BadRequest("Package not found");

            bool result = await _packageRepository.DeleteAsync(exisitingEntity);
            if (!result) return ServiceResult<bool>.Error("Failed to delete package");

            return ServiceResult<bool>.NoContent();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to delete package");
        }
    }

    public async Task<ServiceResult<IEnumerable<Package>>> GetAllAsync()
    {
        try
        {
            var entities = await _packageRepository.GetAllAsync();
            var mapped = entities.Select(x => x.MapTo<Package>()).ToList();

            return ServiceResult<IEnumerable<Package>>.Ok(mapped);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<IEnumerable<Package>>.Error("Failed to fetch Packages");
        }
    }

    public async Task<ServiceResult<Package>> GetAsync(int id)
    {
        try
        {
            var exisitingEntity = await _packageRepository.GetAsync(findBy: x => x.Id == id); ;
            if (exisitingEntity is null) return ServiceResult<Package>.BadRequest("package not found");

            return ServiceResult<Package>.Ok(exisitingEntity.MapTo<Package>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Package>.Error("Failed to fetch Package");
        }
    }

    public async Task<ServiceResult<Package>> UpdateAsync(int id, PackageForm form)
    {
        try
        {
            var exisitingEntity = await _packageRepository.GetAsync(findBy: x => x.Id == id);
            if (exisitingEntity is null) return ServiceResult<Package>.BadRequest("Package not found");

            exisitingEntity.Name = form.Name;
            exisitingEntity.Benefits = form.Benefits;
            exisitingEntity.NumberOfTickets = form.NumberOfTickets;
            exisitingEntity.ExtraFeeInProcent = form.ExtraFeeInProcent;
            exisitingEntity.IsSeating = form.IsSeating;

            var updatedEntity = await _packageRepository.UpdateAsync(exisitingEntity); ;
            if (updatedEntity is null) return ServiceResult<Package>.Error("Failed to update package");

            return ServiceResult<Package>.Ok(updatedEntity.MapTo<Package>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Package>.Error("Failed to update package");
        }
    }
}
